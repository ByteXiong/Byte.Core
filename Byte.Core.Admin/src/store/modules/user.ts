import { resetRouter } from "@/router";
import { store } from "@/store";

import { defineStore } from "pinia";
import "@/api";
import { LoginParam } from "@/api/globals";
import defaultSettings from "@/settings";

export const useUserStore = defineStore("user", () => {
  interface LoginInfoDTO {
    /**
     * 主键Id!
     */
    id?: string;
    /**
     * 名称
     */
    name?: string;
    /**
     * 头像
     */
    avatar?: string;
    /**
     * 账号
     */
    account?: string;
    roles?: string[];
    perms?: string[];
  }

  const user: LoginInfoDTO = {
    roles: [],
    perms: [],
  };

  // 获取信息(用户昵称、头像、角色集合、权限集合)
  const { send: getUserInfo } = useRequest(
    () =>
      Apis.Login.get_api_login_info({
        transform: ({ data }) => {
          // if (!data) {
          //   reject("Verification failed, please Login again.");
          //   return;
          // }
          // if (!data.roles || data.roles.length <= 0) {
          //   reject("getUserInfo: roles must be a non-null array!");
          //   return;
          // }
          Object.assign(user, { ...data });
          return data;
        },
      }),
    {
      immediate: false,
    }
  );
  /**
   *
   * @returns 退出登录
   */

  const { send: logout } = useRequest(
    () =>
      Apis.Login.post_api_login_loginout({
        transform: () => {
          localStorage.setItem(defaultSettings.tokenKey, "");
          location.reload(); // 清空路由
        },
      }),
    {
      immediate: false,
    }
  );

  // remove token
  function resetToken() {
    return new Promise<void>((resolve) => {
      localStorage.setItem("accessToken", "");
      resetRouter();
      resolve();
    });
  }

  return {
    user,
    getUserInfo,
    logout,
    resetToken,
  };
});

// 非setup
export function useUserStoreHook() {
  return useUserStore(store);
}
