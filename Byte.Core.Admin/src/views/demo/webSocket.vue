<template>
  <div class="list_main_content">
    <el-row>
      <el-col :span="6">
        <el-card shadow="never" :header="`在线用户编号-${id}`">
          <el-input v-model="wsurl" placeholder="请输入内容"></el-input>
          <div class="flex">
            <div>{{ socket.status }}</div>
            <el-button type="primary" @click="openWebSocket"> 连接</el-button>
            <el-button type="primary" @click="closeWebSocket"> 断开</el-button>
          </div>
        </el-card>
      </el-col>
      <el-col :span="18">
        <el-card shadow="never" header="WebSocket">
          <p>{{ messageLog }}</p>
          <template #footer>
            <div class="flex">
              <el-input v-model="message"></el-input>
              <el-button type="primary" @click="messageWebSocket">
                发送</el-button
              >
            </div>
          </template>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, useId } from "vue";
const id = useId();
const wsurl = ref("ws://127.0.0.1:3000/api/Websocket/Connect?socketId=" + id);

const socket = useWebSocket(wsurl.value, {
  heartbeat: {
    message: "ping",
    interval: 1000,
    pongTimeout: 1000,
  },
  autoReconnect: {
    retries: 3,
    delay: 1000,
    onFailed() {
      alert("Failed to connect WebSocket after 3 retries");
    },
  },
});
const message = ref("");
const isConnected = ref(false);

const openWebSocket = () => {
  console.log("打开", socket.status.value);
  socket.open();
}; // 监听消息事件
const messageWebSocket = () => {
  socket.send(message.value);
};

// socket.send = (event) => {
//   message.value = event.data;
// };
const messageLog = ref("");
const closeWebSocket = () => {
  socket.close();
};
watch(
  () => socket.data.value,
  (newVal) => {
    messageLog.value += newVal + "\t";
  }
);
</script>
