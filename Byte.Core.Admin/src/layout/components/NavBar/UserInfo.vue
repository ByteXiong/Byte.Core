<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    destroy-on-close
    @close="close"
  />
</template>
<script setup lang="ts"></script>
