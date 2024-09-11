<template>
  <div class="list_main_content">
    <el-row>
      <el-col :span="6">
        <el-card shadow="never" header="在线用户">
          <div v-for="item in user" :key="item" type="success">{{ item }}</div>
        </el-card>
      </el-col>
      <el-col :span="18">
        <el-card shadow="never" header="WebSocket">
          <div class="flex">
            <el-input v-model="wsurl" placeholder="请输入内容" />
            <div>{{ socket.status }}</div>
            <el-button type="primary" @click="openWebSocket"> 连接</el-button>
            <el-button type="primary" @click="closeWebSocket"> 断开</el-button>
          </div>
          <p>{{ messageLog }}</p>
          <template #footer>
            <div class="flex">
              <el-input v-model="message" />
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
import { ref } from "vue";
import moment from "moment";
const wsurl = ref(
  "ws://bytexiong.fun/api/Websocket/Connect?socketId=" + moment().unix()
);
const user = ref<string[]>([]);

const socket = useWebSocket(wsurl.value, {
  heartbeat: {
    message: '{"Type":1,"Data":"ping"}',
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
    let data = JSON.parse(newVal);
    user.value = data.Data;
    messageLog.value += newVal + "\t";
  }
);
</script>
