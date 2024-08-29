import { createAlova } from "alova";
import { axiosRequestAdapter } from "@alova/adapter-axios";
import vueHook from "alova/vue";
import { createApis, withConfigType } from "./createApis";
import defaultSettings from "@/settings";
import { useUserStoreHook } from "@/store/modules/user";
type ICodeMessage = {
  [key: number]: string;
};
enum ResultEnum {
  SUCCESS = 0,
  TIMEOUT = 401,
}
const serverCodeMessage: ICodeMessage = {
  200: "服务器成功返回请求的数据",
  401: "无用户信息,请登录",
  403: "Forbidden",
  404: "网络请求错误,未找到该资源",
  500: "服务器发生错误，请检查服务器(Internal Server Error)",
  502: "网关错误(Bad Gateway)",
  503: "服务不可用，服务器暂时过载或维护(Service Unavailable)",
  504: "网关超时(Gateway Timeout)",
  547: "数据关联,删除失败",
};

export const alovaInstance = createAlova({
  baseURL: import.meta.env.VITE_PROXY_PREFIX,
  statesHook: vueHook,
  requestAdapter: axiosRequestAdapter(),
  beforeRequest: (method) => {
    method.config.headers.Authorization = localStorage.getItem(
      defaultSettings.tokenKey
    );
    method.config.headers["api-version"] = 1.0;
  },
  responded: {
    onSuccess: async (response, method) => {
      // @ts-ignore
      const { status, data: rawData } = response;
      if (status === 200) {
        if (rawData.code === ResultEnum.SUCCESS) {
          return rawData as any;
        } else if (rawData.code === ResultEnum.TIMEOUT) {
          ElMessageBox.confirm("当前页面已失效，请重新登录", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          }).then(() => {
            const userStore = useUserStoreHook();
            userStore.resetToken().then(() => {
              location.reload();
            });
          });
        }
        ElMessage.error(
          rawData.msg || serverCodeMessage[rawData.code] || "系统出错"
        );
        return Promise.reject(rawData);
      }
      return Promise.reject(rawData);
    },
    onError: (err, method) => {
      ElMessage.error(err || "系统出错");
      return Promise.reject({ err, method });
    },
    onComplete: async (method) => {
      // 处理请求完成逻辑
    },
  },
});

export const $$userConfigMap = withConfigType({});

const Apis = createApis(alovaInstance, $$userConfigMap);

export default Apis;
