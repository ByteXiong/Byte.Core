<template>
  <div class="login-container">
    <!-- 顶部 -->
    <div class="absolute-lt flex-x-end p-3 w-full">
      <el-switch
        v-model="isDark"
        inline-prompt
        :active-icon="Moon"
        :inactive-icon="Sunny"
        @change="toggleTheme"
      />
      <lang-select class="ml-2 cursor-pointer" />
    </div>
    <div
      class="typing-container"
      ref="typingContainer"
      style="line-height: normal; height: auto"
    ></div>

    <!-- 登录表单 -->
    <el-card
      class="absolute right-30 items-center !border-none !bg-transparent !rounded-4% w-100 <sm:w-85"
    >
      <div class="text-center relative">
        <h2>{{ defaultSettings.title }}</h2>
        <el-tag class="ml-2 absolute-rt">{{ defaultSettings.version }}</el-tag>
      </div>

      <el-form
        ref="loginFormRef"
        :model="loginData"
        :rules="loginRules"
        class="login-form"
      >
        <!-- 用户名 -->
        <el-form-item prop="username">
          <div class="flex-y-center w-full">
            <svg-icon icon-class="user" class="mx-2" />
            <el-input
              ref="username"
              v-model="loginData.account"
              :placeholder="$t('login.username')"
              name="username"
              size="large"
              class="h-[48px]"
            />
          </div>
        </el-form-item>

        <!-- 密码 -->
        <el-tooltip
          :visible="isCapslock"
          content="Caps lock is On"
          placement="right"
        >
          <el-form-item prop="password">
            <div class="flex-y-center w-full">
              <el-icon class="mx-2"><Lock /></el-icon>
              <el-input
                v-model="loginData.password"
                :placeholder="$t('login.password')"
                type="password"
                name="password"
                @keyup="checkCapslock"
                @keyup.enter="handleLogin"
                size="large"
                class="h-[48px] pr-2"
                show-password
              />
            </div>
          </el-form-item>
        </el-tooltip>

        <!-- 验证码 -->
        <el-form-item prop="captchaCode">
          <div class="flex-y-center w-full">
            <svg-icon icon-class="captcha" class="mx-2" />
            <el-input
              v-model="loginData.captchaCode"
              auto-complete="off"
              size="large"
              class="flex-1"
              :placeholder="$t('login.captchaCode')"
              @keyup.enter="handleLogin"
            />

            <el-image
              @click="getCaptcha"
              :src="captchaBase64"
              class="rounded-tr-md rounded-br-md cursor-pointer h-[48px]"
            />
          </div>
        </el-form-item>

        <!-- 登录按钮 -->
        <el-button
          :loading="loading"
          type="primary"
          size="large"
          class="w-full"
          @click.prevent="handleLogin"
          >{{ $t("login.login") }}
        </el-button>

        <!-- 账号密码提示 -->
        <div class="mt-10 text-sm">
          <span>{{ $t("login.username") }}: admin</span>
          <span class="ml-4"> {{ $t("login.password") }}: 123456</span>
        </div>
        <div class="mt-10 text">
          <a target="_blank" href="https://gitee.com/ByteXiong/Byte.Core">
            gittee :https://gitee.com/ByteXiong/Byte.Core</a
          >
        </div>
        <div class="mt-10 text">
          <a
            target="_blank"
            href="http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=o0kj4P6WcaHOJeHIvdfqmlkZnRhGsljY&authKey=O%2Fhfpj%2B63L3OSf4Xz1aPBcSDWlVXJ%2FaicDUwHtqF3vwAKB0hCLnIU%2FKks500F3mj&noverify=0&group_code=929412850"
          >
            Byte.Core QQ交流群:929412850</a
          >
        </div>
      </el-form>
    </el-card>
    <!-- ICP备案 -->
    <div class="absolute bottom-1 text-[10px] text-center" v-show="icpVisible">
      <p>
        Copyright © 2024 - 2025 bytexiong.fun All Rights Reserved. 个人
        版权所有
      </p>
      <p>蜀ICP备2024077653号</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useSettingsStore, useUserStore, useAppStore } from "@/store";
import { Sunny, Moon } from "@element-plus/icons-vue";
import { LocationQuery, LocationQueryValue, useRoute } from "vue-router";
import router from "@/router";
import defaultSettings from "@/settings";
import { ThemeEnum } from "@/enums/ThemeEnum";
import { useI18n } from "vue-i18n";
import "@/api";
import { LoginParam } from "@/api/globals";
// Stores
const userStore = useUserStore();
const settingsStore = useSettingsStore();
const appStore = useAppStore();

// Internationalization
const { t } = useI18n();

// Reactive states
const isDark = ref(settingsStore.theme === ThemeEnum.DARK);
const icpVisible = ref(true);
const isCapslock = ref(false); // 是否大写锁定
const captchaBase64 = ref(); // 验证码图片Base64字符串
const loginFormRef = ref(ElForm); // 登录表单ref
const { height } = useWindowSize();

const route = useRoute();
/**
 * 获取验证码
 */
const { send: getCaptcha } = useRequest(
  () =>
    Apis.Login.get_api_login_captcha({
      transform: (res) => {
        loginData.value.captchaId = res.data.captchaId;
        captchaBase64.value = res.data.img;
      },
    }),
  {
    immediate: true,
  }
);

const {
  send: login,
  loading,
  onError: nologin,
} = useRequest(
  (loginData: LoginParam) =>
    Apis.Login.post_api_login_login({
      data: loginData,
      transform: (res) => {
        const { tokenType, accessToken } = res.data;
        localStorage.setItem(
          defaultSettings.tokenKey,
          tokenType + " " + accessToken
        ); // Bearer eyJhbGciOiJIUzI1NiJ9.xxx.xxx

        const query: LocationQuery = route.query;
        const redirect = (query.redirect as LocationQueryValue) ?? "/";
        const otherQueryParams = Object.keys(query).reduce(
          (acc: any, cur: string) => {
            if (cur !== "redirect") {
              acc[cur] = query[cur];
            }
            return acc;
          },
          {}
        );

        router.push({ path: redirect, query: otherQueryParams });

        return res;
      },
    }),
  {
    immediate: false,
  }
);

nologin(() => {
  getCaptcha();
});
const loginData = ref<LoginParam>({
  account: "admin",
  password: "123456",
  captchaCode: "6",
});

const loginRules = computed(() => {
  const prefix = appStore.language === "en" ? "Please enter " : "请输入";
  return {
    account: [
      {
        required: true,
        trigger: "blur",
        message: `${prefix}${t("login.username")}`,
      },
    ],
    password: [
      {
        required: true,
        trigger: "blur",
        validator: (rule: any, value: any, callback: any) => {
          if (value.length < 6) {
            callback(new Error("The password can not be less than 6 digits"));
          } else {
            callback();
          }
        },
        message: `${prefix}${t("login.password")}`,
      },
    ],
    captchaCode: [
      {
        required: true,
        trigger: "blur",
        message: `${prefix}${t("login.captchaCode")}`,
      },
    ],
  };
});

/**
 * 登录
 */

function handleLogin() {
  loginFormRef.value.validate((valid: boolean) => {
    if (valid) {
      login(loginData.value);
    }
  });
}

/**
 * 主题切换
 */

const toggleTheme = () => {
  const newTheme =
    settingsStore.theme === ThemeEnum.DARK ? ThemeEnum.LIGHT : ThemeEnum.DARK;
  settingsStore.changeTheme(newTheme);
};
/**
 * 根据屏幕宽度切换设备模式
 */

watchEffect(() => {
  if (height.value < 600) {
    icpVisible.value = false;
  } else {
    icpVisible.value = true;
  }
});

/**
 * 检查输入大小写
 */
function checkCapslock(e: any) {
  isCapslock.value = e.getModifierState("CapsLock");
}

const typingContainer = ref();
const text = ref(
  `大佬您好！
    欢迎莅临 Byte.Core，我们致力于重新定义敏捷开发。
    在项目框架方面，我们紧跟时代潮流，采用了.net8 与 vue3 ts 技术。
    而在技术层面，Byte.Core 基于 SqlSugar 和 Alova.js，让交互变得更为轻松便捷。
    在如今这个 Gitee、Github 开源框架层出不穷，AI 盛行的时代，为何我们还要推出开源框架呢？\n
    这是为了解决诸多开发痛点：
        1.代码存在 质量低，可读性差，可维护性不佳。
        2.例如：一些代码逻辑混乱，变量命名不规范，导致后续开发者难以理解和修改。
        3.接口文档缺乏注释，前端开发人员常常一脸茫然，还得辛苦地为字段编写注释。
        4.前后端分离导致沟通成本居高不下，容易出现互相推诿的情况。
        5.就像前后端对需求理解不一致，引发诸多矛盾。
        6.后端修改接口却未及时通知前端，有时甚至后端自己都记不清修改的内容。\n
    而我们的优势在于：
        1.保持开源。
        2.切实解决开发过程中的痛点。
        3.后端框架实现集中化管理（已发布至 NGet），开发者只需专注于业务，无需操心框架问题。
        4.前端框架采用 Alova.js 配合 OpenAPI，能够实时关注接口动向。\n
    乾坤未定，你我皆是黑马。欢迎兄弟加入！\n
    無人扶我青云志，我自踏雪至山巅。作者开源不易，恳请您点个星支持一下。\n
    目前项目仍在开发中，敬请期待......`
);
const index = ref(0);

function typeEffect() {
  if (index.value < text.value.length) {
    const char = text.value.charAt(index.value);
    typingContainer.value.innerHTML += text.value.substring(
      index.value,
      index.value + 1
    );
    index.value++;
    setTimeout(typeEffect, 50); // 调整打字速度
  }
  // else {
  //   setTimeout(() => {
  //     index.value = 0;
  //     typingContainer.value.innerHTML = "";
  //     typeEffect();
  //   }, 2000); // 调整循环延迟
  // }
}
onMounted(() => {
  typeEffect();
});
</script>

<style lang="scss" scoped>
html.dark .login-container {
  background: url("@/assets/images/login-bg-dark.jpg") no-repeat center right;
}

.login-container {
  overflow-y: auto;
  background: url("@/assets/images/login-bg.jpg") no-repeat center right;

  @apply wh-full flex-center;

  .login-form {
    padding: 30px 10px;
  }
}

.el-form-item {
  background: var(--el-input-bg-color);
  border: 1px solid var(--el-border-color);
  border-radius: 5px;
}

:deep(.el-input) {
  .el-input__wrapper {
    padding: 0;
    background-color: transparent;
    box-shadow: none;

    &.is-focus,
    &:hover {
      box-shadow: none !important;
    }

    input:-webkit-autofill {
      /* 通过延时渲染背景色变相去除背景颜色 */
      transition: background-color 1000s ease-in-out 0s;
    }
  }
}

.typing-container {
  height: 100%;
  width: 60%;
  position: absolute;
  left: 20px;
  top: 80px;
  font-family: monospace;
  font-size: 24px; /* 字体大小 */
  font-weight: bold; /* 字体加粗 */
  white-space: pre-wrap;
  overflow: hidden;
  // border-right: 3px solid; /* 光标粗细 */
  // animation: cursor-blink 0.7s steps(1, start) infinite;
}

// @keyframes cursor-blink {
//   0%,
//   100% {
//     border-color: transparent;
//   }
//   50% {
//     border-color: black;
//   }
// }
</style>
