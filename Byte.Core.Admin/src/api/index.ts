import { createAlova } from "alova";
import { axiosRequestAdapter } from "@alova/adapter-axios";
import vueHook from "alova/vue";
import { createApis, withConfigType } from "./createApis";
import defaultSettings from "@/settings";
import { useUserStoreHook } from "@/store/modules/user";
export enum ResultEnum {
  SUCCESS = 0,
  TIMEOUT = 401,
}

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
    // 请求成功的拦截器
    // 当使用GlobalFetch请求适配器时，第一个参数接收Response对象
    // 第二个参数为当前请求的method实例，你可以用它同步请求前后的配置信息
    onSuccess: async (response, method) => {
      // @ts-ignore
      const { status, data: rawData } = response;
      if (status === 200) {
        // if (enableDownload) {
        //     // 下载处理
        //     return rawData;
        // }
        // if (enableUpload) {
        //     // 上传处理
        //     return rawData;
        // }
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
        ElMessage.error(rawData.msg || "系统出错");
        return Promise.reject(rawData);
      }
      return Promise.reject(rawData);
    },

    // 请求失败的拦截器
    // 请求错误时将会进入该拦截器。
    // 第二个参数为当前请求的method实例，你可以用它同步请求前后的配置信息
    onError: (err, method) => {
      ElMessage.error(err || "系统出错");
      return Promise.reject({ err, method });
    },

    // 请求完成的拦截器
    // 当你需要在请求不论是成功、失败、还是命中缓存都需要执行的逻辑时，可以在创建alova实例时指定全局的`onComplete`拦截器，例如关闭请求 loading 状态。
    // 接收当前请求的method实例
    onComplete: async (method) => {
      // 处理请求完成逻辑
    },
  },
});

export const $$userConfigMap = withConfigType({});

const Apis = createApis(alovaInstance, $$userConfigMap);

export default Apis;
