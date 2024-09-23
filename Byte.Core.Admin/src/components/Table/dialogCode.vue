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
    <span>function customRender(row, h) {</span>
    <v-ace-editor
      v-model:value="modelValue"
      lang="json"
      theme="github"
      :options="options"
      class="vue-ace-editor"
    />
    <span> }</span>
    <template #footer>
      <el-button @click="modelValue = ''">清空</el-button>

      <el-button
        type="primary"
        @click="() => (visible = false)"
        :loading="loading"
      >
        暂存
      </el-button>
    </template>
  </el-dialog>
  <slot :open="open"></slot>
</template>
<script lang="ts" setup>
import { ElMessage, FormInstance } from "element-plus";
import { ref, onMounted, onUnmounted } from "vue";
import { VAceEditor } from "vue3-ace-editor";
import "./aceConfig.ts";
import type { Ace } from "ace-builds";
const title = ref("代码");
const visible = ref(false);
const loading = ref(false);
const modelValue = defineModel<string>() as Ref<string>;
const close = () => {
  // cmRef.value?.refresh();
  visible.value = false;
};
const open = () => {
  visible.value = true;
};
const handleSubmit = () => {};

// const cmRef = ref<CmComponentRef>();
const content = ref(""); // 显示的内容

const options: Partial<Ace.EditorOptions> = reactive({
  useWorker: true, // 启用语法检查,必须为true
  enableBasicAutocompletion: true, // 自动补全
  enableLiveAutocompletion: true, // 智能补全
  enableSnippets: true, // 启用代码段
  showPrintMargin: false, // 去掉灰色的线，printMarginColumn
  highlightActiveLine: true, // 高亮行
  highlightSelectedWord: true, // 高亮选中的字符
  tabSize: 4, // tab锁进字符
  fontSize: 14, // 设置字号
  wrap: false, // 是否换行
  readonly: false, // 是否可编辑
  // minLines: 10, // 最小行数，minLines和maxLines同时设置之后，可以不用给editor再设置高度
  // maxLines: 50, // 最大行数
});
const openCode = (str: string) => {
  visible.value = true;
};

// const onChange = (val: string, cm: Editor) => {
//   console.log(val);
//   console.log(cm.getValue());
// };

// const onInput = (val: string) => {
//   console.log(val);
// };

// const onReady = (cm: Editor) => {
//   console.log(cm.focus());
// };

// onMounted(() => {
//   setTimeout(() => {
//     cmRef.value?.refresh();
//   }, 1000);

//   setTimeout(() => {
//     cmRef.value?.resize(300, 200);
//   }, 2000);

//   setTimeout(() => {
//     cmRef.value?.cminstance.isClean();
//   }, 3000);
// });

// onUnmounted(() => {
//   cmRef.value?.destroy();
// });
defineExpose({
  openCode,
  visible,
});
</script>
<style scoped>
.vue-ace-editor {
  /* ace-editor默认没有高度，所以必须设置高度，或者同时设置最小行和最大行使编辑器的高度自动增高 */
  height: 300px;
  width: 100%;
  font-size: 16px;
  border: 1px solid;
}
</style>
