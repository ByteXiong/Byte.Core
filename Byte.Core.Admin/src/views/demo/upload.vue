<template>
  <div class="list_main_content">
    <el-card>
      <template #header> 上传图片组件 后端压缩测试 </template>
      <avatar-upLoad v-model="uploadUrl" />
      URl地址: {{ uploadUrl }}
    </el-card>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import AvatarUpLoad from "@/components/Upload/AvatarUpLoad.vue";

const uploadUrl = ref("");
</script>

<style scoped>
.avatar-uploader .avatar {
  width: 178px;
  height: 178px;
  display: block;
}
</style>

<style>
.avatar-uploader .el-upload {
  border: 1px dashed var(--el-border-color);
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: var(--el-transition-duration-fast);
}

.avatar-uploader .el-upload:hover {
  border-color: var(--el-color-primary);
}

.el-icon.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  text-align: center;
}
</style>
