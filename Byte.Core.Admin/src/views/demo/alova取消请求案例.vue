<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <el-button type="primary" @click="send()">发送请求</el-button>
      <!-- {{ data }} -->

      <el-button type="primary" :loading="!loading" @click="abort()"
        >请求中断</el-button
      >

      <!-- <el-button type="primary" @click="aa()">method</el-button> -->
    </el-card>
  </div>
</template>
<script setup lang="ts">
import { alovaInstance } from "@/api";
import { Method } from "alova";

const { data, loading, send, abort } = useRequest(
  () =>
    Apis.Demo.get_api_demo_cancelrequest({
      transform: (res) => {
        console.log(res);

        return res.data;
      },
    }),
  {
    immediate: false,
  }
);
</script>
