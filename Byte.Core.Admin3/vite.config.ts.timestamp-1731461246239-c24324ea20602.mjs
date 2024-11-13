// vite.config.ts
import process3 from "node:process";
import { URL, fileURLToPath } from "node:url";
import { defineConfig, loadEnv } from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0/node_modules/vite/dist/node/index.js";

// build/plugins/index.ts
import vue from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@vitejs+plugin-vue@5.1.4_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0__vue@3.5.12_typescript@5.6.3_/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import vueJsx from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@vitejs+plugin-vue-jsx@4.0.1_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0__vue@3.5.12_typescript@5.6.3_/node_modules/@vitejs/plugin-vue-jsx/dist/index.mjs";
import VueDevtools from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/vite-plugin-vue-devtools@7.5.4_rollup@4.24.0_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terse_so3sqqygjzn4bk4gbfvec5bktm/node_modules/vite-plugin-vue-devtools/dist/vite.mjs";
import progress from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/vite-plugin-progress@0.0.7_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0_/node_modules/vite-plugin-progress/dist/index.mjs";

// build/plugins/router.ts
import ElegantVueRouter from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@elegant-router+vue@0.3.8/node_modules/@elegant-router/vue/dist/vite.mjs";
function setupElegantRouter() {
  return ElegantVueRouter({
    layouts: {
      base: "src/layouts/base-layout/index.vue",
      blank: "src/layouts/blank-layout/index.vue"
    },
    customRoutes: {
      names: [
        "exception_403",
        "exception_404",
        "exception_500",
        "document_project",
        "document_project-link",
        "document_vue",
        "document_vite",
        "document_unocss",
        "document_naive",
        "document_antd",
        "document_alova"
      ]
    },
    routePathTransformer(routeName, routePath) {
      const key = routeName;
      if (key === "login") {
        const modules = ["pwd-login", "code-login", "register", "reset-pwd", "bind-wechat"];
        const moduleReg = modules.join("|");
        return `/login/:module(${moduleReg})?`;
      }
      return routePath;
    },
    onRouteMetaGen(routeName) {
      const key = routeName;
      const constantRoutes = ["login", "403", "404", "500"];
      const meta = {
        title: key,
        i18nKey: `route.${key}`
      };
      if (constantRoutes.includes(key)) {
        meta.constant = true;
      }
      return meta;
    }
  });
}

// build/plugins/unocss.ts
import process from "node:process";
import path from "node:path";
import unocss from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@unocss+vite@0.63.6_rollup@4.24.0_typescript@5.6.3_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0_/node_modules/@unocss/vite/dist/index.mjs";
import presetIcons from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@unocss+preset-icons@0.63.6/node_modules/@unocss/preset-icons/dist/index.mjs";
import { FileSystemIconLoader } from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/@iconify+utils@2.1.33/node_modules/@iconify/utils/lib/loader/node-loaders.mjs";
function setupUnocss(viteEnv) {
  const { VITE_ICON_PREFIX, VITE_ICON_LOCAL_PREFIX } = viteEnv;
  const localIconPath = path.join(process.cwd(), "src/assets/svg-icon");
  const collectionName = VITE_ICON_LOCAL_PREFIX.replace(`${VITE_ICON_PREFIX}-`, "");
  return unocss({
    presets: [
      presetIcons({
        prefix: `${VITE_ICON_PREFIX}-`,
        scale: 1,
        extraProperties: {
          display: "inline-block"
        },
        collections: {
          [collectionName]: FileSystemIconLoader(
            localIconPath,
            (svg) => svg.replace(/^<svg\s/, '<svg width="1em" height="1em" ')
          )
        },
        warn: true
      })
    ]
  });
}

// build/plugins/unplugin.ts
import process2 from "node:process";
import path2 from "node:path";
import Icons from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/unplugin-icons@0.19.3_@vue+compiler-sfc@3.5.12_webpack-sources@3.2.3/node_modules/unplugin-icons/dist/vite.js";
import IconsResolver from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/unplugin-icons@0.19.3_@vue+compiler-sfc@3.5.12_webpack-sources@3.2.3/node_modules/unplugin-icons/dist/resolver.js";
import Components from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/unplugin-vue-components@0.27.4_@babel+parser@7.25.8_rollup@4.24.0_vue@3.5.12_typescript@5.6.3__webpack-sources@3.2.3/node_modules/unplugin-vue-components/dist/vite.js";
import { AntDesignVueResolver, NaiveUiResolver } from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/unplugin-vue-components@0.27.4_@babel+parser@7.25.8_rollup@4.24.0_vue@3.5.12_typescript@5.6.3__webpack-sources@3.2.3/node_modules/unplugin-vue-components/dist/resolvers.js";
import { FileSystemIconLoader as FileSystemIconLoader2 } from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/unplugin-icons@0.19.3_@vue+compiler-sfc@3.5.12_webpack-sources@3.2.3/node_modules/unplugin-icons/dist/loaders.js";
import { createSvgIconsPlugin } from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/vite-plugin-svg-icons@2.0.1_vite@5.4.10_@types+node@22.7.9_sass@1.80.4_terser@5.36.0_/node_modules/vite-plugin-svg-icons/dist/index.mjs";
function setupUnplugin(viteEnv) {
  const { VITE_ICON_PREFIX, VITE_ICON_LOCAL_PREFIX } = viteEnv;
  const localIconPath = path2.join(process2.cwd(), "src/assets/svg-icon");
  const collectionName = VITE_ICON_LOCAL_PREFIX.replace(`${VITE_ICON_PREFIX}-`, "");
  const plugins = [
    Icons({
      compiler: "vue3",
      customCollections: {
        [collectionName]: FileSystemIconLoader2(
          localIconPath,
          (svg) => svg.replace(/^<svg\s/, '<svg width="1em" height="1em" ')
        )
      },
      scale: 1,
      defaultClass: "inline-block"
    }),
    Components({
      dts: "src/typings/components.d.ts",
      types: [{ from: "vue-router", names: ["RouterLink", "RouterView"] }],
      resolvers: [
        AntDesignVueResolver({
          importStyle: false
        }),
        NaiveUiResolver(),
        IconsResolver({ customCollections: [collectionName], componentPrefix: VITE_ICON_PREFIX })
      ]
    }),
    createSvgIconsPlugin({
      iconDirs: [localIconPath],
      symbolId: `${VITE_ICON_LOCAL_PREFIX}-[dir]-[name]`,
      inject: "body-last",
      customDomId: "__SVG_ICON_LOCAL__"
    })
  ];
  return plugins;
}

// build/plugins/html.ts
function setupHtmlPlugin(buildTime) {
  const plugin = {
    name: "html-plugin",
    apply: "build",
    transformIndexHtml(html) {
      return html.replace("<head>", `<head>
    <meta name="buildTime" content="${buildTime}">`);
    }
  };
  return plugin;
}

// build/plugins/index.ts
function setupVitePlugins(viteEnv, buildTime) {
  const plugins = [
    vue(),
    vueJsx(),
    VueDevtools(),
    setupElegantRouter(),
    setupUnocss(viteEnv),
    ...setupUnplugin(viteEnv),
    progress(),
    setupHtmlPlugin(buildTime)
  ];
  return plugins;
}

// src/utils/service.ts
import json5 from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/json5@2.2.3/node_modules/json5/lib/index.js";
function createServiceConfig(env) {
  const { VITE_SERVICE_BASE_URL, VITE_OTHER_SERVICE_BASE_URL } = env;
  let other = {};
  try {
    other = json5.parse(VITE_OTHER_SERVICE_BASE_URL);
  } catch {
    console.error("VITE_OTHER_SERVICE_BASE_URL is not a valid json5 string");
  }
  const httpConfig = {
    baseURL: VITE_SERVICE_BASE_URL,
    other
  };
  const otherHttpKeys = Object.keys(httpConfig.other);
  const otherConfig = otherHttpKeys.map((key) => {
    return {
      key,
      baseURL: httpConfig.other[key],
      proxyPattern: createProxyPattern(key)
    };
  });
  const config = {
    baseURL: httpConfig.baseURL,
    proxyPattern: createProxyPattern(),
    other: otherConfig
  };
  return config;
}
function createProxyPattern(key) {
  if (!key) {
    return "/proxy-default";
  }
  return `/proxy-${key}`;
}

// build/config/proxy.ts
function createViteProxy(env, enable) {
  const isEnableHttpProxy = enable && env.VITE_HTTP_PROXY === "Y";
  if (!isEnableHttpProxy) return void 0;
  const { baseURL, proxyPattern, other } = createServiceConfig(env);
  const proxy = createProxyItem({ baseURL, proxyPattern });
  other.forEach((item) => {
    Object.assign(proxy, createProxyItem(item));
  });
  return proxy;
}
function createProxyItem(item) {
  const proxy = {};
  proxy[item.proxyPattern] = {
    target: item.baseURL,
    changeOrigin: true,
    rewrite: (path3) => path3.replace(new RegExp(`^${item.proxyPattern}`), "")
  };
  return proxy;
}

// build/config/time.ts
import dayjs from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/dayjs@1.11.13/node_modules/dayjs/dayjs.min.js";
import utc from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/dayjs@1.11.13/node_modules/dayjs/plugin/utc.js";
import timezone from "file:///E:/Byte.Core/Byte.Core.Admin3/node_modules/.pnpm/dayjs@1.11.13/node_modules/dayjs/plugin/timezone.js";
function getBuildTime() {
  dayjs.extend(utc);
  dayjs.extend(timezone);
  const buildTime = dayjs.tz(Date.now(), "Asia/Shanghai").format("YYYY-MM-DD HH:mm:ss");
  return buildTime;
}

// vite.config.ts
var __vite_injected_original_import_meta_url = "file:///E:/Byte.Core/Byte.Core.Admin3/vite.config.ts";
var vite_config_default = defineConfig((configEnv) => {
  const viteEnv = loadEnv(configEnv.mode, process3.cwd());
  const buildTime = getBuildTime();
  const enableProxy = configEnv.command === "serve" && !configEnv.isPreview;
  return {
    base: viteEnv.VITE_BASE_URL,
    resolve: {
      alias: {
        "~": fileURLToPath(new URL("./", __vite_injected_original_import_meta_url)),
        "@": fileURLToPath(new URL("./src", __vite_injected_original_import_meta_url))
      }
    },
    css: {
      preprocessorOptions: {
        scss: {
          api: "modern-compiler",
          additionalData: `@use "@/styles/scss/global.scss" as *;`
        }
      }
    },
    plugins: setupVitePlugins(viteEnv, buildTime),
    define: {
      BUILD_TIME: JSON.stringify(buildTime)
    },
    server: {
      host: "0.0.0.0",
      port: 3001,
      open: true,
      proxy: createViteProxy(viteEnv, enableProxy),
      fs: {
        cachedChecks: false
      }
    },
    preview: {
      port: 9725
    },
    optimizeDeps: {
      include: [
        `monaco-editor/esm/vs/language/json/json.worker`,
        `monaco-editor/esm/vs/language/css/css.worker`,
        `monaco-editor/esm/vs/language/html/html.worker`,
        `monaco-editor/esm/vs/language/typescript/ts.worker`,
        `monaco-editor/esm/vs/editor/editor.worker`
      ]
    },
    build: {
      reportCompressedSize: false,
      sourcemap: viteEnv.VITE_SOURCE_MAP === "Y",
      commonjsOptions: {
        ignoreTryCatch: false
      }
    }
  };
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcudHMiLCAiYnVpbGQvcGx1Z2lucy9pbmRleC50cyIsICJidWlsZC9wbHVnaW5zL3JvdXRlci50cyIsICJidWlsZC9wbHVnaW5zL3Vub2Nzcy50cyIsICJidWlsZC9wbHVnaW5zL3VucGx1Z2luLnRzIiwgImJ1aWxkL3BsdWdpbnMvaHRtbC50cyIsICJzcmMvdXRpbHMvc2VydmljZS50cyIsICJidWlsZC9jb25maWcvcHJveHkudHMiLCAiYnVpbGQvY29uZmlnL3RpbWUudHMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9maWxlbmFtZSA9IFwiRTpcXFxcQnl0ZS5Db3JlXFxcXEJ5dGUuQ29yZS5BZG1pbjNcXFxcdml0ZS5jb25maWcudHNcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfaW1wb3J0X21ldGFfdXJsID0gXCJmaWxlOi8vL0U6L0J5dGUuQ29yZS9CeXRlLkNvcmUuQWRtaW4zL3ZpdGUuY29uZmlnLnRzXCI7aW1wb3J0IHByb2Nlc3MgZnJvbSAnbm9kZTpwcm9jZXNzJztcbmltcG9ydCB7IFVSTCwgZmlsZVVSTFRvUGF0aCB9IGZyb20gJ25vZGU6dXJsJztcbmltcG9ydCB7IGRlZmluZUNvbmZpZywgbG9hZEVudiB9IGZyb20gJ3ZpdGUnO1xuaW1wb3J0IHsgc2V0dXBWaXRlUGx1Z2lucyB9IGZyb20gJy4vYnVpbGQvcGx1Z2lucyc7XG5pbXBvcnQgeyBjcmVhdGVWaXRlUHJveHksIGdldEJ1aWxkVGltZSB9IGZyb20gJy4vYnVpbGQvY29uZmlnJztcblxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKGNvbmZpZ0VudiA9PiB7XG4gIGNvbnN0IHZpdGVFbnYgPSBsb2FkRW52KGNvbmZpZ0Vudi5tb2RlLCBwcm9jZXNzLmN3ZCgpKSBhcyB1bmtub3duIGFzIEVudi5JbXBvcnRNZXRhO1xuXG4gIGNvbnN0IGJ1aWxkVGltZSA9IGdldEJ1aWxkVGltZSgpO1xuXG4gIGNvbnN0IGVuYWJsZVByb3h5ID0gY29uZmlnRW52LmNvbW1hbmQgPT09ICdzZXJ2ZScgJiYgIWNvbmZpZ0Vudi5pc1ByZXZpZXc7XG5cbiAgcmV0dXJuIHtcbiAgICBiYXNlOiB2aXRlRW52LlZJVEVfQkFTRV9VUkwsXG4gICAgcmVzb2x2ZToge1xuICAgICAgYWxpYXM6IHtcbiAgICAgICAgJ34nOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoJy4vJywgaW1wb3J0Lm1ldGEudXJsKSksXG4gICAgICAgICdAJzogZmlsZVVSTFRvUGF0aChuZXcgVVJMKCcuL3NyYycsIGltcG9ydC5tZXRhLnVybCkpXG4gICAgICB9XG4gICAgfSxcbiAgICBjc3M6IHtcbiAgICAgIHByZXByb2Nlc3Nvck9wdGlvbnM6IHtcbiAgICAgICAgc2Nzczoge1xuICAgICAgICAgIGFwaTogJ21vZGVybi1jb21waWxlcicsXG4gICAgICAgICAgYWRkaXRpb25hbERhdGE6IGBAdXNlIFwiQC9zdHlsZXMvc2Nzcy9nbG9iYWwuc2Nzc1wiIGFzICo7YFxuICAgICAgICB9XG4gICAgICB9XG4gICAgfSxcbiAgICBwbHVnaW5zOiBzZXR1cFZpdGVQbHVnaW5zKHZpdGVFbnYsIGJ1aWxkVGltZSksXG4gICAgZGVmaW5lOiB7XG4gICAgICBCVUlMRF9USU1FOiBKU09OLnN0cmluZ2lmeShidWlsZFRpbWUpXG4gICAgfSxcbiAgICBzZXJ2ZXI6IHtcbiAgICAgIGhvc3Q6ICcwLjAuMC4wJyxcbiAgICAgIHBvcnQ6IDMwMDEsXG4gICAgICBvcGVuOiB0cnVlLFxuICAgICAgcHJveHk6IGNyZWF0ZVZpdGVQcm94eSh2aXRlRW52LCBlbmFibGVQcm94eSksXG4gICAgICBmczoge1xuICAgICAgICBjYWNoZWRDaGVja3M6IGZhbHNlXG4gICAgICB9XG4gICAgfSxcbiAgICBwcmV2aWV3OiB7XG4gICAgICBwb3J0OiA5NzI1XG4gICAgfSxcbiAgICBvcHRpbWl6ZURlcHM6IHtcbiAgICAgIGluY2x1ZGU6IFtcbiAgICAgICAgYG1vbmFjby1lZGl0b3IvZXNtL3ZzL2xhbmd1YWdlL2pzb24vanNvbi53b3JrZXJgLFxuICAgICAgICBgbW9uYWNvLWVkaXRvci9lc20vdnMvbGFuZ3VhZ2UvY3NzL2Nzcy53b3JrZXJgLFxuICAgICAgICBgbW9uYWNvLWVkaXRvci9lc20vdnMvbGFuZ3VhZ2UvaHRtbC9odG1sLndvcmtlcmAsXG4gICAgICAgIGBtb25hY28tZWRpdG9yL2VzbS92cy9sYW5ndWFnZS90eXBlc2NyaXB0L3RzLndvcmtlcmAsXG4gICAgICAgIGBtb25hY28tZWRpdG9yL2VzbS92cy9lZGl0b3IvZWRpdG9yLndvcmtlcmBcbiAgICAgIF1cbiAgICB9LFxuICAgIGJ1aWxkOiB7XG4gICAgICByZXBvcnRDb21wcmVzc2VkU2l6ZTogZmFsc2UsXG4gICAgICBzb3VyY2VtYXA6IHZpdGVFbnYuVklURV9TT1VSQ0VfTUFQID09PSAnWScsXG4gICAgICBjb21tb25qc09wdGlvbnM6IHtcbiAgICAgICAgaWdub3JlVHJ5Q2F0Y2g6IGZhbHNlXG4gICAgICB9XG4gICAgfVxuICB9O1xufSk7XG4iLCAiY29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2Rpcm5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXHBsdWdpbnNcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXHBsdWdpbnNcXFxcaW5kZXgudHNcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfaW1wb3J0X21ldGFfdXJsID0gXCJmaWxlOi8vL0U6L0J5dGUuQ29yZS9CeXRlLkNvcmUuQWRtaW4zL2J1aWxkL3BsdWdpbnMvaW5kZXgudHNcIjtpbXBvcnQgdHlwZSB7IFBsdWdpbk9wdGlvbiB9IGZyb20gJ3ZpdGUnO1xuaW1wb3J0IHZ1ZSBmcm9tICdAdml0ZWpzL3BsdWdpbi12dWUnO1xuaW1wb3J0IHZ1ZUpzeCBmcm9tICdAdml0ZWpzL3BsdWdpbi12dWUtanN4JztcbmltcG9ydCBWdWVEZXZ0b29scyBmcm9tICd2aXRlLXBsdWdpbi12dWUtZGV2dG9vbHMnO1xuaW1wb3J0IHByb2dyZXNzIGZyb20gJ3ZpdGUtcGx1Z2luLXByb2dyZXNzJztcbmltcG9ydCB7IHNldHVwRWxlZ2FudFJvdXRlciB9IGZyb20gJy4vcm91dGVyJztcbmltcG9ydCB7IHNldHVwVW5vY3NzIH0gZnJvbSAnLi91bm9jc3MnO1xuaW1wb3J0IHsgc2V0dXBVbnBsdWdpbiB9IGZyb20gJy4vdW5wbHVnaW4nO1xuaW1wb3J0IHsgc2V0dXBIdG1sUGx1Z2luIH0gZnJvbSAnLi9odG1sJztcblxuZXhwb3J0IGZ1bmN0aW9uIHNldHVwVml0ZVBsdWdpbnModml0ZUVudjogRW52LkltcG9ydE1ldGEsIGJ1aWxkVGltZTogc3RyaW5nKSB7XG4gIGNvbnN0IHBsdWdpbnM6IFBsdWdpbk9wdGlvbiA9IFtcbiAgICB2dWUoKSxcbiAgICB2dWVKc3goKSxcbiAgICBWdWVEZXZ0b29scygpLFxuICAgIHNldHVwRWxlZ2FudFJvdXRlcigpLFxuICAgIHNldHVwVW5vY3NzKHZpdGVFbnYpLFxuICAgIC4uLnNldHVwVW5wbHVnaW4odml0ZUVudiksXG4gICAgcHJvZ3Jlc3MoKSxcbiAgICBzZXR1cEh0bWxQbHVnaW4oYnVpbGRUaW1lKVxuICBdO1xuXG4gIHJldHVybiBwbHVnaW5zO1xufVxuIiwgImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxidWlsZFxcXFxwbHVnaW5zXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxidWlsZFxcXFxwbHVnaW5zXFxcXHJvdXRlci50c1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vRTovQnl0ZS5Db3JlL0J5dGUuQ29yZS5BZG1pbjMvYnVpbGQvcGx1Z2lucy9yb3V0ZXIudHNcIjtpbXBvcnQgdHlwZSB7IFJvdXRlTWV0YSB9IGZyb20gJ3Z1ZS1yb3V0ZXInO1xuaW1wb3J0IEVsZWdhbnRWdWVSb3V0ZXIgZnJvbSAnQGVsZWdhbnQtcm91dGVyL3Z1ZS92aXRlJztcbmltcG9ydCB0eXBlIHsgUm91dGVLZXkgfSBmcm9tICdAZWxlZ2FudC1yb3V0ZXIvdHlwZXMnO1xuXG5leHBvcnQgZnVuY3Rpb24gc2V0dXBFbGVnYW50Um91dGVyKCkge1xuICByZXR1cm4gRWxlZ2FudFZ1ZVJvdXRlcih7XG4gICAgbGF5b3V0czoge1xuICAgICAgYmFzZTogJ3NyYy9sYXlvdXRzL2Jhc2UtbGF5b3V0L2luZGV4LnZ1ZScsXG4gICAgICBibGFuazogJ3NyYy9sYXlvdXRzL2JsYW5rLWxheW91dC9pbmRleC52dWUnXG4gICAgfSxcbiAgICBjdXN0b21Sb3V0ZXM6IHtcbiAgICAgIG5hbWVzOiBbXG4gICAgICAgICdleGNlcHRpb25fNDAzJyxcbiAgICAgICAgJ2V4Y2VwdGlvbl80MDQnLFxuICAgICAgICAnZXhjZXB0aW9uXzUwMCcsXG4gICAgICAgICdkb2N1bWVudF9wcm9qZWN0JyxcbiAgICAgICAgJ2RvY3VtZW50X3Byb2plY3QtbGluaycsXG4gICAgICAgICdkb2N1bWVudF92dWUnLFxuICAgICAgICAnZG9jdW1lbnRfdml0ZScsXG4gICAgICAgICdkb2N1bWVudF91bm9jc3MnLFxuICAgICAgICAnZG9jdW1lbnRfbmFpdmUnLFxuICAgICAgICAnZG9jdW1lbnRfYW50ZCcsXG4gICAgICAgICdkb2N1bWVudF9hbG92YSdcbiAgICAgIF1cbiAgICB9LFxuICAgIHJvdXRlUGF0aFRyYW5zZm9ybWVyKHJvdXRlTmFtZSwgcm91dGVQYXRoKSB7XG4gICAgICBjb25zdCBrZXkgPSByb3V0ZU5hbWUgYXMgUm91dGVLZXk7XG5cbiAgICAgIGlmIChrZXkgPT09ICdsb2dpbicpIHtcbiAgICAgICAgY29uc3QgbW9kdWxlczogVW5pb25LZXkuTG9naW5Nb2R1bGVbXSA9IFsncHdkLWxvZ2luJywgJ2NvZGUtbG9naW4nLCAncmVnaXN0ZXInLCAncmVzZXQtcHdkJywgJ2JpbmQtd2VjaGF0J107XG5cbiAgICAgICAgY29uc3QgbW9kdWxlUmVnID0gbW9kdWxlcy5qb2luKCd8Jyk7XG5cbiAgICAgICAgcmV0dXJuIGAvbG9naW4vOm1vZHVsZSgke21vZHVsZVJlZ30pP2A7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiByb3V0ZVBhdGg7XG4gICAgfSxcbiAgICBvblJvdXRlTWV0YUdlbihyb3V0ZU5hbWUpIHtcbiAgICAgIGNvbnN0IGtleSA9IHJvdXRlTmFtZSBhcyBSb3V0ZUtleTtcblxuICAgICAgY29uc3QgY29uc3RhbnRSb3V0ZXM6IFJvdXRlS2V5W10gPSBbJ2xvZ2luJywgJzQwMycsICc0MDQnLCAnNTAwJ107XG5cbiAgICAgIGNvbnN0IG1ldGE6IFBhcnRpYWw8Um91dGVNZXRhPiA9IHtcbiAgICAgICAgdGl0bGU6IGtleSxcbiAgICAgICAgaTE4bktleTogYHJvdXRlLiR7a2V5fWAgYXMgQXBwLkkxOG4uSTE4bktleVxuICAgICAgfTtcblxuICAgICAgaWYgKGNvbnN0YW50Um91dGVzLmluY2x1ZGVzKGtleSkpIHtcbiAgICAgICAgbWV0YS5jb25zdGFudCA9IHRydWU7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBtZXRhO1xuICAgIH1cbiAgfSk7XG59XG4iLCAiY29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2Rpcm5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXHBsdWdpbnNcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXHBsdWdpbnNcXFxcdW5vY3NzLnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9FOi9CeXRlLkNvcmUvQnl0ZS5Db3JlLkFkbWluMy9idWlsZC9wbHVnaW5zL3Vub2Nzcy50c1wiO2ltcG9ydCBwcm9jZXNzIGZyb20gJ25vZGU6cHJvY2Vzcyc7XG5pbXBvcnQgcGF0aCBmcm9tICdub2RlOnBhdGgnO1xuaW1wb3J0IHVub2NzcyBmcm9tICdAdW5vY3NzL3ZpdGUnO1xuaW1wb3J0IHByZXNldEljb25zIGZyb20gJ0B1bm9jc3MvcHJlc2V0LWljb25zJztcbmltcG9ydCB7IEZpbGVTeXN0ZW1JY29uTG9hZGVyIH0gZnJvbSAnQGljb25pZnkvdXRpbHMvbGliL2xvYWRlci9ub2RlLWxvYWRlcnMnO1xuXG5leHBvcnQgZnVuY3Rpb24gc2V0dXBVbm9jc3Modml0ZUVudjogRW52LkltcG9ydE1ldGEpIHtcbiAgY29uc3QgeyBWSVRFX0lDT05fUFJFRklYLCBWSVRFX0lDT05fTE9DQUxfUFJFRklYIH0gPSB2aXRlRW52O1xuXG4gIGNvbnN0IGxvY2FsSWNvblBhdGggPSBwYXRoLmpvaW4ocHJvY2Vzcy5jd2QoKSwgJ3NyYy9hc3NldHMvc3ZnLWljb24nKTtcblxuICAvKiogVGhlIG5hbWUgb2YgdGhlIGxvY2FsIGljb24gY29sbGVjdGlvbiAqL1xuICBjb25zdCBjb2xsZWN0aW9uTmFtZSA9IFZJVEVfSUNPTl9MT0NBTF9QUkVGSVgucmVwbGFjZShgJHtWSVRFX0lDT05fUFJFRklYfS1gLCAnJyk7XG5cbiAgcmV0dXJuIHVub2Nzcyh7XG4gICAgcHJlc2V0czogW1xuICAgICAgcHJlc2V0SWNvbnMoe1xuICAgICAgICBwcmVmaXg6IGAke1ZJVEVfSUNPTl9QUkVGSVh9LWAsXG4gICAgICAgIHNjYWxlOiAxLFxuICAgICAgICBleHRyYVByb3BlcnRpZXM6IHtcbiAgICAgICAgICBkaXNwbGF5OiAnaW5saW5lLWJsb2NrJ1xuICAgICAgICB9LFxuICAgICAgICBjb2xsZWN0aW9uczoge1xuICAgICAgICAgIFtjb2xsZWN0aW9uTmFtZV06IEZpbGVTeXN0ZW1JY29uTG9hZGVyKGxvY2FsSWNvblBhdGgsIHN2ZyA9PlxuICAgICAgICAgICAgc3ZnLnJlcGxhY2UoL148c3ZnXFxzLywgJzxzdmcgd2lkdGg9XCIxZW1cIiBoZWlnaHQ9XCIxZW1cIiAnKVxuICAgICAgICAgIClcbiAgICAgICAgfSxcbiAgICAgICAgd2FybjogdHJ1ZVxuICAgICAgfSlcbiAgICBdXG4gIH0pO1xufVxuIiwgImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxidWlsZFxcXFxwbHVnaW5zXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxidWlsZFxcXFxwbHVnaW5zXFxcXHVucGx1Z2luLnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9FOi9CeXRlLkNvcmUvQnl0ZS5Db3JlLkFkbWluMy9idWlsZC9wbHVnaW5zL3VucGx1Z2luLnRzXCI7aW1wb3J0IHByb2Nlc3MgZnJvbSAnbm9kZTpwcm9jZXNzJztcbmltcG9ydCBwYXRoIGZyb20gJ25vZGU6cGF0aCc7XG5pbXBvcnQgdHlwZSB7IFBsdWdpbk9wdGlvbiB9IGZyb20gJ3ZpdGUnO1xuaW1wb3J0IEljb25zIGZyb20gJ3VucGx1Z2luLWljb25zL3ZpdGUnO1xuaW1wb3J0IEljb25zUmVzb2x2ZXIgZnJvbSAndW5wbHVnaW4taWNvbnMvcmVzb2x2ZXInO1xuaW1wb3J0IENvbXBvbmVudHMgZnJvbSAndW5wbHVnaW4tdnVlLWNvbXBvbmVudHMvdml0ZSc7XG5pbXBvcnQgeyBBbnREZXNpZ25WdWVSZXNvbHZlciwgTmFpdmVVaVJlc29sdmVyIH0gZnJvbSAndW5wbHVnaW4tdnVlLWNvbXBvbmVudHMvcmVzb2x2ZXJzJztcbmltcG9ydCB7IEZpbGVTeXN0ZW1JY29uTG9hZGVyIH0gZnJvbSAndW5wbHVnaW4taWNvbnMvbG9hZGVycyc7XG5pbXBvcnQgeyBjcmVhdGVTdmdJY29uc1BsdWdpbiB9IGZyb20gJ3ZpdGUtcGx1Z2luLXN2Zy1pY29ucyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBzZXR1cFVucGx1Z2luKHZpdGVFbnY6IEVudi5JbXBvcnRNZXRhKSB7XG4gIGNvbnN0IHsgVklURV9JQ09OX1BSRUZJWCwgVklURV9JQ09OX0xPQ0FMX1BSRUZJWCB9ID0gdml0ZUVudjtcblxuICBjb25zdCBsb2NhbEljb25QYXRoID0gcGF0aC5qb2luKHByb2Nlc3MuY3dkKCksICdzcmMvYXNzZXRzL3N2Zy1pY29uJyk7XG5cbiAgLyoqIFRoZSBuYW1lIG9mIHRoZSBsb2NhbCBpY29uIGNvbGxlY3Rpb24gKi9cbiAgY29uc3QgY29sbGVjdGlvbk5hbWUgPSBWSVRFX0lDT05fTE9DQUxfUFJFRklYLnJlcGxhY2UoYCR7VklURV9JQ09OX1BSRUZJWH0tYCwgJycpO1xuXG4gIGNvbnN0IHBsdWdpbnM6IFBsdWdpbk9wdGlvbltdID0gW1xuICAgIEljb25zKHtcbiAgICAgIGNvbXBpbGVyOiAndnVlMycsXG4gICAgICBjdXN0b21Db2xsZWN0aW9uczoge1xuICAgICAgICBbY29sbGVjdGlvbk5hbWVdOiBGaWxlU3lzdGVtSWNvbkxvYWRlcihsb2NhbEljb25QYXRoLCBzdmcgPT5cbiAgICAgICAgICBzdmcucmVwbGFjZSgvXjxzdmdcXHMvLCAnPHN2ZyB3aWR0aD1cIjFlbVwiIGhlaWdodD1cIjFlbVwiICcpXG4gICAgICAgIClcbiAgICAgIH0sXG4gICAgICBzY2FsZTogMSxcbiAgICAgIGRlZmF1bHRDbGFzczogJ2lubGluZS1ibG9jaydcbiAgICB9KSxcbiAgICBDb21wb25lbnRzKHtcbiAgICAgIGR0czogJ3NyYy90eXBpbmdzL2NvbXBvbmVudHMuZC50cycsXG4gICAgICB0eXBlczogW3sgZnJvbTogJ3Z1ZS1yb3V0ZXInLCBuYW1lczogWydSb3V0ZXJMaW5rJywgJ1JvdXRlclZpZXcnXSB9XSxcbiAgICAgIHJlc29sdmVyczogW1xuICAgICAgICBBbnREZXNpZ25WdWVSZXNvbHZlcih7XG4gICAgICAgICAgaW1wb3J0U3R5bGU6IGZhbHNlXG4gICAgICAgIH0pLFxuICAgICAgICBOYWl2ZVVpUmVzb2x2ZXIoKSxcbiAgICAgICAgSWNvbnNSZXNvbHZlcih7IGN1c3RvbUNvbGxlY3Rpb25zOiBbY29sbGVjdGlvbk5hbWVdLCBjb21wb25lbnRQcmVmaXg6IFZJVEVfSUNPTl9QUkVGSVggfSlcbiAgICAgIF1cbiAgICB9KSxcbiAgICBjcmVhdGVTdmdJY29uc1BsdWdpbih7XG4gICAgICBpY29uRGlyczogW2xvY2FsSWNvblBhdGhdLFxuICAgICAgc3ltYm9sSWQ6IGAke1ZJVEVfSUNPTl9MT0NBTF9QUkVGSVh9LVtkaXJdLVtuYW1lXWAsXG4gICAgICBpbmplY3Q6ICdib2R5LWxhc3QnLFxuICAgICAgY3VzdG9tRG9tSWQ6ICdfX1NWR19JQ09OX0xPQ0FMX18nXG4gICAgfSlcbiAgXTtcblxuICByZXR1cm4gcGx1Z2lucztcbn1cbiIsICJjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZGlybmFtZSA9IFwiRTpcXFxcQnl0ZS5Db3JlXFxcXEJ5dGUuQ29yZS5BZG1pbjNcXFxcYnVpbGRcXFxccGx1Z2luc1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9maWxlbmFtZSA9IFwiRTpcXFxcQnl0ZS5Db3JlXFxcXEJ5dGUuQ29yZS5BZG1pbjNcXFxcYnVpbGRcXFxccGx1Z2luc1xcXFxodG1sLnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9FOi9CeXRlLkNvcmUvQnl0ZS5Db3JlLkFkbWluMy9idWlsZC9wbHVnaW5zL2h0bWwudHNcIjtpbXBvcnQgdHlwZSB7IFBsdWdpbiB9IGZyb20gJ3ZpdGUnO1xuXG5leHBvcnQgZnVuY3Rpb24gc2V0dXBIdG1sUGx1Z2luKGJ1aWxkVGltZTogc3RyaW5nKSB7XG4gIGNvbnN0IHBsdWdpbjogUGx1Z2luID0ge1xuICAgIG5hbWU6ICdodG1sLXBsdWdpbicsXG4gICAgYXBwbHk6ICdidWlsZCcsXG4gICAgdHJhbnNmb3JtSW5kZXhIdG1sKGh0bWwpIHtcbiAgICAgIHJldHVybiBodG1sLnJlcGxhY2UoJzxoZWFkPicsIGA8aGVhZD5cXG4gICAgPG1ldGEgbmFtZT1cImJ1aWxkVGltZVwiIGNvbnRlbnQ9XCIke2J1aWxkVGltZX1cIj5gKTtcbiAgICB9XG4gIH07XG5cbiAgcmV0dXJuIHBsdWdpbjtcbn1cbiIsICJjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZGlybmFtZSA9IFwiRTpcXFxcQnl0ZS5Db3JlXFxcXEJ5dGUuQ29yZS5BZG1pbjNcXFxcc3JjXFxcXHV0aWxzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxzcmNcXFxcdXRpbHNcXFxcc2VydmljZS50c1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vRTovQnl0ZS5Db3JlL0J5dGUuQ29yZS5BZG1pbjMvc3JjL3V0aWxzL3NlcnZpY2UudHNcIjtpbXBvcnQganNvbjUgZnJvbSAnanNvbjUnO1xuXG4vKipcbiAqIENyZWF0ZSBzZXJ2aWNlIGNvbmZpZyBieSBjdXJyZW50IGVudlxuICpcbiAqIEBwYXJhbSBlbnYgVGhlIGN1cnJlbnQgZW52XG4gKi9cbmV4cG9ydCBmdW5jdGlvbiBjcmVhdGVTZXJ2aWNlQ29uZmlnKGVudjogRW52LkltcG9ydE1ldGEpIHtcbiAgY29uc3QgeyBWSVRFX1NFUlZJQ0VfQkFTRV9VUkwsIFZJVEVfT1RIRVJfU0VSVklDRV9CQVNFX1VSTCB9ID0gZW52O1xuXG4gIGxldCBvdGhlciA9IHt9IGFzIFJlY29yZDxBcHAuU2VydmljZS5PdGhlckJhc2VVUkxLZXksIHN0cmluZz47XG4gIHRyeSB7XG4gICAgb3RoZXIgPSBqc29uNS5wYXJzZShWSVRFX09USEVSX1NFUlZJQ0VfQkFTRV9VUkwpO1xuICB9IGNhdGNoIHtcbiAgICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8tY29uc29sZVxuICAgIGNvbnNvbGUuZXJyb3IoJ1ZJVEVfT1RIRVJfU0VSVklDRV9CQVNFX1VSTCBpcyBub3QgYSB2YWxpZCBqc29uNSBzdHJpbmcnKTtcbiAgfVxuXG4gIGNvbnN0IGh0dHBDb25maWc6IEFwcC5TZXJ2aWNlLlNpbXBsZVNlcnZpY2VDb25maWcgPSB7XG4gICAgYmFzZVVSTDogVklURV9TRVJWSUNFX0JBU0VfVVJMLFxuICAgIG90aGVyXG4gIH07XG5cbiAgY29uc3Qgb3RoZXJIdHRwS2V5cyA9IE9iamVjdC5rZXlzKGh0dHBDb25maWcub3RoZXIpIGFzIEFwcC5TZXJ2aWNlLk90aGVyQmFzZVVSTEtleVtdO1xuXG4gIGNvbnN0IG90aGVyQ29uZmlnOiBBcHAuU2VydmljZS5PdGhlclNlcnZpY2VDb25maWdJdGVtW10gPSBvdGhlckh0dHBLZXlzLm1hcChrZXkgPT4ge1xuICAgIHJldHVybiB7XG4gICAgICBrZXksXG4gICAgICBiYXNlVVJMOiBodHRwQ29uZmlnLm90aGVyW2tleV0sXG4gICAgICBwcm94eVBhdHRlcm46IGNyZWF0ZVByb3h5UGF0dGVybihrZXkpXG4gICAgfTtcbiAgfSk7XG5cbiAgY29uc3QgY29uZmlnOiBBcHAuU2VydmljZS5TZXJ2aWNlQ29uZmlnID0ge1xuICAgIGJhc2VVUkw6IGh0dHBDb25maWcuYmFzZVVSTCxcbiAgICBwcm94eVBhdHRlcm46IGNyZWF0ZVByb3h5UGF0dGVybigpLFxuICAgIG90aGVyOiBvdGhlckNvbmZpZ1xuICB9O1xuXG4gIHJldHVybiBjb25maWc7XG59XG5cbi8qKlxuICogZ2V0IGJhY2tlbmQgc2VydmljZSBiYXNlIHVybFxuICpcbiAqIEBwYXJhbSBlbnYgLSB0aGUgY3VycmVudCBlbnZcbiAqIEBwYXJhbSBpc1Byb3h5IC0gaWYgdXNlIHByb3h5XG4gKi9cbmV4cG9ydCBmdW5jdGlvbiBnZXRTZXJ2aWNlQmFzZVVSTChlbnY6IEVudi5JbXBvcnRNZXRhLCBpc1Byb3h5OiBib29sZWFuKSB7XG4gIGNvbnN0IHsgYmFzZVVSTCwgb3RoZXIgfSA9IGNyZWF0ZVNlcnZpY2VDb25maWcoZW52KTtcblxuICBjb25zdCBvdGhlckJhc2VVUkwgPSB7fSBhcyBSZWNvcmQ8QXBwLlNlcnZpY2UuT3RoZXJCYXNlVVJMS2V5LCBzdHJpbmc+O1xuXG4gIG90aGVyLmZvckVhY2goaXRlbSA9PiB7XG4gICAgb3RoZXJCYXNlVVJMW2l0ZW0ua2V5XSA9IGlzUHJveHkgPyBpdGVtLnByb3h5UGF0dGVybiA6IGl0ZW0uYmFzZVVSTDtcbiAgfSk7XG5cbiAgcmV0dXJuIHtcbiAgICBiYXNlVVJMOiBpc1Byb3h5ID8gY3JlYXRlUHJveHlQYXR0ZXJuKCkgOiBiYXNlVVJMLFxuICAgIG90aGVyQmFzZVVSTFxuICB9O1xufVxuXG4vKipcbiAqIEdldCBwcm94eSBwYXR0ZXJuIG9mIGJhY2tlbmQgc2VydmljZSBiYXNlIHVybFxuICpcbiAqIEBwYXJhbSBrZXkgSWYgbm90IHNldCwgd2lsbCB1c2UgdGhlIGRlZmF1bHQga2V5XG4gKi9cbmZ1bmN0aW9uIGNyZWF0ZVByb3h5UGF0dGVybihrZXk/OiBBcHAuU2VydmljZS5PdGhlckJhc2VVUkxLZXkpIHtcbiAgaWYgKCFrZXkpIHtcbiAgICByZXR1cm4gJy9wcm94eS1kZWZhdWx0JztcbiAgfVxuXG4gIHJldHVybiBgL3Byb3h5LSR7a2V5fWA7XG59XG4iLCAiY29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2Rpcm5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXGNvbmZpZ1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9maWxlbmFtZSA9IFwiRTpcXFxcQnl0ZS5Db3JlXFxcXEJ5dGUuQ29yZS5BZG1pbjNcXFxcYnVpbGRcXFxcY29uZmlnXFxcXHByb3h5LnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9FOi9CeXRlLkNvcmUvQnl0ZS5Db3JlLkFkbWluMy9idWlsZC9jb25maWcvcHJveHkudHNcIjtpbXBvcnQgdHlwZSB7IFByb3h5T3B0aW9ucyB9IGZyb20gJ3ZpdGUnO1xuaW1wb3J0IHsgY3JlYXRlU2VydmljZUNvbmZpZyB9IGZyb20gJy4uLy4uL3NyYy91dGlscy9zZXJ2aWNlJztcblxuLyoqXG4gKiBTZXQgaHR0cCBwcm94eVxuICpcbiAqIEBwYXJhbSBlbnYgLSBUaGUgY3VycmVudCBlbnZcbiAqIEBwYXJhbSBlbmFibGUgLSBJZiBlbmFibGUgaHR0cCBwcm94eVxuICovXG5leHBvcnQgZnVuY3Rpb24gY3JlYXRlVml0ZVByb3h5KGVudjogRW52LkltcG9ydE1ldGEsIGVuYWJsZTogYm9vbGVhbikge1xuICBjb25zdCBpc0VuYWJsZUh0dHBQcm94eSA9IGVuYWJsZSAmJiBlbnYuVklURV9IVFRQX1BST1hZID09PSAnWSc7XG5cbiAgaWYgKCFpc0VuYWJsZUh0dHBQcm94eSkgcmV0dXJuIHVuZGVmaW5lZDtcblxuICBjb25zdCB7IGJhc2VVUkwsIHByb3h5UGF0dGVybiwgb3RoZXIgfSA9IGNyZWF0ZVNlcnZpY2VDb25maWcoZW52KTtcblxuICBjb25zdCBwcm94eTogUmVjb3JkPHN0cmluZywgUHJveHlPcHRpb25zPiA9IGNyZWF0ZVByb3h5SXRlbSh7IGJhc2VVUkwsIHByb3h5UGF0dGVybiB9KTtcblxuICBvdGhlci5mb3JFYWNoKGl0ZW0gPT4ge1xuICAgIE9iamVjdC5hc3NpZ24ocHJveHksIGNyZWF0ZVByb3h5SXRlbShpdGVtKSk7XG4gIH0pO1xuXG4gIHJldHVybiBwcm94eTtcbn1cblxuZnVuY3Rpb24gY3JlYXRlUHJveHlJdGVtKGl0ZW06IEFwcC5TZXJ2aWNlLlNlcnZpY2VDb25maWdJdGVtKSB7XG4gIGNvbnN0IHByb3h5OiBSZWNvcmQ8c3RyaW5nLCBQcm94eU9wdGlvbnM+ID0ge307XG5cbiAgcHJveHlbaXRlbS5wcm94eVBhdHRlcm5dID0ge1xuICAgIHRhcmdldDogaXRlbS5iYXNlVVJMLFxuICAgIGNoYW5nZU9yaWdpbjogdHJ1ZSxcbiAgICByZXdyaXRlOiBwYXRoID0+IHBhdGgucmVwbGFjZShuZXcgUmVnRXhwKGBeJHtpdGVtLnByb3h5UGF0dGVybn1gKSwgJycpXG4gIH07XG5cbiAgcmV0dXJuIHByb3h5O1xufVxuIiwgImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJFOlxcXFxCeXRlLkNvcmVcXFxcQnl0ZS5Db3JlLkFkbWluM1xcXFxidWlsZFxcXFxjb25maWdcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIkU6XFxcXEJ5dGUuQ29yZVxcXFxCeXRlLkNvcmUuQWRtaW4zXFxcXGJ1aWxkXFxcXGNvbmZpZ1xcXFx0aW1lLnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9FOi9CeXRlLkNvcmUvQnl0ZS5Db3JlLkFkbWluMy9idWlsZC9jb25maWcvdGltZS50c1wiO2ltcG9ydCBkYXlqcyBmcm9tICdkYXlqcyc7XG5pbXBvcnQgdXRjIGZyb20gJ2RheWpzL3BsdWdpbi91dGMnO1xuaW1wb3J0IHRpbWV6b25lIGZyb20gJ2RheWpzL3BsdWdpbi90aW1lem9uZSc7XG5cbmV4cG9ydCBmdW5jdGlvbiBnZXRCdWlsZFRpbWUoKSB7XG4gIGRheWpzLmV4dGVuZCh1dGMpO1xuICBkYXlqcy5leHRlbmQodGltZXpvbmUpO1xuXG4gIGNvbnN0IGJ1aWxkVGltZSA9IGRheWpzLnR6KERhdGUubm93KCksICdBc2lhL1NoYW5naGFpJykuZm9ybWF0KCdZWVlZLU1NLUREIEhIOm1tOnNzJyk7XG5cbiAgcmV0dXJuIGJ1aWxkVGltZTtcbn1cbiJdLAogICJtYXBwaW5ncyI6ICI7QUFBK1EsT0FBT0EsY0FBYTtBQUNuUyxTQUFTLEtBQUsscUJBQXFCO0FBQ25DLFNBQVMsY0FBYyxlQUFlOzs7QUNEdEMsT0FBTyxTQUFTO0FBQ2hCLE9BQU8sWUFBWTtBQUNuQixPQUFPLGlCQUFpQjtBQUN4QixPQUFPLGNBQWM7OztBQ0hyQixPQUFPLHNCQUFzQjtBQUd0QixTQUFTLHFCQUFxQjtBQUNuQyxTQUFPLGlCQUFpQjtBQUFBLElBQ3RCLFNBQVM7QUFBQSxNQUNQLE1BQU07QUFBQSxNQUNOLE9BQU87QUFBQSxJQUNUO0FBQUEsSUFDQSxjQUFjO0FBQUEsTUFDWixPQUFPO0FBQUEsUUFDTDtBQUFBLFFBQ0E7QUFBQSxRQUNBO0FBQUEsUUFDQTtBQUFBLFFBQ0E7QUFBQSxRQUNBO0FBQUEsUUFDQTtBQUFBLFFBQ0E7QUFBQSxRQUNBO0FBQUEsUUFDQTtBQUFBLFFBQ0E7QUFBQSxNQUNGO0FBQUEsSUFDRjtBQUFBLElBQ0EscUJBQXFCLFdBQVcsV0FBVztBQUN6QyxZQUFNLE1BQU07QUFFWixVQUFJLFFBQVEsU0FBUztBQUNuQixjQUFNLFVBQWtDLENBQUMsYUFBYSxjQUFjLFlBQVksYUFBYSxhQUFhO0FBRTFHLGNBQU0sWUFBWSxRQUFRLEtBQUssR0FBRztBQUVsQyxlQUFPLGtCQUFrQixTQUFTO0FBQUEsTUFDcEM7QUFFQSxhQUFPO0FBQUEsSUFDVDtBQUFBLElBQ0EsZUFBZSxXQUFXO0FBQ3hCLFlBQU0sTUFBTTtBQUVaLFlBQU0saUJBQTZCLENBQUMsU0FBUyxPQUFPLE9BQU8sS0FBSztBQUVoRSxZQUFNLE9BQTJCO0FBQUEsUUFDL0IsT0FBTztBQUFBLFFBQ1AsU0FBUyxTQUFTLEdBQUc7QUFBQSxNQUN2QjtBQUVBLFVBQUksZUFBZSxTQUFTLEdBQUcsR0FBRztBQUNoQyxhQUFLLFdBQVc7QUFBQSxNQUNsQjtBQUVBLGFBQU87QUFB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LFFBQ2xCO0FBQUEsTUFDRjtBQUFBLElBQ0Y7QUFBQSxJQUNBLFNBQVMsaUJBQWlCLFNBQVMsU0FBUztBQUFBLElBQzVDLFFBQVE7QUFBQSxNQUNOLFlBQVksS0FBSyxVQUFVLFNBQVM7QUFBQSxJQUN0QztBQUFBLElBQ0EsUUFBUTtBQUFBLE1BQ04sTUFBTTtBQUFBLE1BQ04sTUFBTTtBQUFBLE1BQ04sTUFBTTtBQUFBLE1BQ04sT0FBTyxnQkFBZ0IsU0FBUyxXQUFXO0FBQUEsTUFDM0MsSUFBSTtBQUFBLFFBQ0YsY0FBYztBQUFBLE1BQ2hCO0FBQUEsSUFDRjtBQUFBLElBQ0EsU0FBUztBQUFBLE1BQ1AsTUFBTTtBQUFBLElBQ1I7QUFBQSxJQUNBLGNBQWM7QUFBQSxNQUNaLFNBQVM7QUFBQSxRQUNQO0FBQUEsUUFDQTtBQUFBLFFBQ0E7QUFBQSxRQUNBO0FBQUEsUUFDQTtBQUFBLE1BQ0Y7QUFBQSxJQUNGO0FBQUEsSUFDQSxPQUFPO0FBQUEsTUFDTCxzQkFBc0I7QUFBQSxNQUN0QixXQUFXLFFBQVEsb0JBQW9CO0FBQUEsTUFDdkMsaUJBQWlCO0FBQUEsUUFDZixnQkFBZ0I7QUFBQSxNQUNsQjtBQUFBLElBQ0Y7QUFBQSxFQUNGO0FBQ0YsQ0FBQzsiLAogICJuYW1lcyI6IFsicHJvY2VzcyIsICJwcm9jZXNzIiwgInBhdGgiLCAiRmlsZVN5c3RlbUljb25Mb2FkZXIiLCAicGF0aCIsICJwcm9jZXNzIiwgIkZpbGVTeXN0ZW1JY29uTG9hZGVyIiwgInBhdGgiLCAicHJvY2VzcyJdCn0K
