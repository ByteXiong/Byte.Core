<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <el-button type="primary" @click="send()">发送请求</el-button>
      <el-button type="primary" @click="abort()">请求取消</el-button>
      {{ data }}
      {{ error }}
    </el-card>
    <el-card shadow="never" class="table-container">
      <el-button type="primary" @click="getSync1()" :loading="loading1"
        >同步请求测试1</el-button
      >
      {{ getSync1Data }}
      <el-button type="primary" @click="getSync2()" :loading="loading2"
        >同步请求测试2</el-button
      >
      {{ getSync2Data }}
    </el-card>
  </div>
</template>
<script setup lang="ts">
import { axiosRequestAdapter } from "@alova/adapter-axios";
import { createAlova, Method } from "alova";
import vueHook from "alova/vue";
import axios from "axios";
const data = ref("");
const loading = ref(false);
const error = ref("");

const alovaInstance = createAlova({
  baseURL: "http://test.ng.bytexiong.fun",
  statesHook: vueHook,
  timeout: 0,
  requestAdapter: axiosRequestAdapter(),
  // beforeRequest: (method) => {
  //   method.config.headers["api-version"] = 1.0;
  // },
});
const method = new Method("GET", alovaInstance, "/api/demo/cancelRequest");
const send = () => {
  method.send();
};
const abort = () => {
  var content = method.abort();
  console.log(method, content);
};

// const controller = new AbortController();
// const send = () => {
//   error.value = "";
//   data.value = "";
//   loading.value = true;
//   axios
//     .get("http://test.ng.bytexiong.fun/api/demo/cancelRequest", {
//       headers: {
//         "Content-Type": "application/json",
//         "api-version": "1.0",
//       },
//       signal: controller.signal,
//     })
//     .then((response) => {
//       loading.value = false;
//       data.value = response.data;
//     })
//     .catch((err) => {
//       loading.value = false;
//       if (axios.isCancel(err)) {
//         error.value = "请求取消";
//       } else {
//         error.value = "异常错误";
//       }
//     })
//     .finally(() => {
//       loading.value = false;
//     });
// };
// const abort = () => {
//   error.value = "";
//   data.value = "";
//   controller.abort();
// };

const {
  data: getSync1Data,
  loading: loading1,
  send: getSync1,
} = useRequest(
  () =>
    Apis.Demo.get_api_demo_getsync({
      transform: (res) => {
        return res.data;
      },
      cacheFor: 0,
    }),
  {
    immediate: false,
  }
);

const {
  data: getSync2Data,
  loading: loading2,
  send: getSync2,
} = useRequest(
  () =>
    Apis.Demo.get_api_demo_getsync({
      transform: (res) => {
        return res.data;
      },
      cacheFor: 0,
    }),
  {
    immediate: false,
  }
);
</script>
