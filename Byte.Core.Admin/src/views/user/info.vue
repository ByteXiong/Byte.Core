<template>
  <el-dialog
    :append-to-body="true"
    :title="title"
    v-model="visible"
    width="80%"
    top="12vh"
    destroy-on-close
    @close="close"
  >
    <div class="h-5xl flex">
      <div class="h-auto w-3/8">头像</div>
      <div class="h-auto">类容</div>
    </div>
  </el-dialog>
</template>
<script setup lang="ts">
const title = ref("个人信息");
const visible = ref(false);
const close = () => {
  visible.value = false;
};
defineExpose({
  visible,
});
</script>
