<template>
  <el-upload
    class="avatar-uploader"
    :action="proxyUrl + '/api/Upload/Images'"
    :headers="headers"
    :show-file-list="false"
    :limit="1"
    :on-success="handleImageSuccess"
    :before-upload="beforeAvatarUpload"
    v-bind="$attrs"
  >
    <img v-if="imageUrl" :src="proxyUrl + imageUrl" class="avatar" />
    <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
  </el-upload>
  <!-- 组件内部地址: {{ props.proxyUrl + imageUrl }} -->
</template>
<script lang="ts" setup>
import { ref, computed } from "vue";
import { ElMessage } from "element-plus";
import { Plus } from "@element-plus/icons-vue";

import type { UploadProps } from "element-plus";
import defaultSettings from "@/settings";

const props = defineProps({
  proxyUrl: {
    type: String,
    default: import.meta.env.VITE_UPLOAD_URL,
  },
  modelValue: {
    type: String,
    default: "",
  },
});

const headers = ref({
  "api-version": "1.0",
  Authorization: localStorage.getItem(defaultSettings.tokenKey),
});
const emit = defineEmits(["update:modelValue"]);
const imageUrl = useVModel(props, "modelValue", emit);
const proxyUrl = useVModel(props, "proxyUrl", emit);

const handleImageSuccess: UploadProps["onSuccess"] = (response, uploadFile) => {
  let { data, success, msg } = response;
  if (success) {
    emit("update:modelValue", data[0] as string);
  } else {
    ElMessage.error(msg);
    emit("update:modelValue", "");
  }
};

const beforeAvatarUpload: UploadProps["beforeUpload"] = (rawFile) => {
  if (rawFile.size / 1024 / 1024 > 2) {
    ElMessage.error("Avatar picture size can not exceed 2MB!");
    return false;
  }
  return true;
};
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
