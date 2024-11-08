<script setup lang="tsx">
import { nextTick, ref } from 'vue';
import * as monaco from 'monaco-editor';
import editorWorker from 'monaco-editor/esm/vs/editor/editor.worker?worker';
import type { TableColumn } from '@/api/globals';

const monacoEditorRef = ref();
const visible = ref(false);
const title = ref('插槽编辑');
const formData = ref<string>('');
const emits = defineEmits(['Change']);
// 解决 Monaco Editor 无法正确加载其所需的 Web Worker
self.MonacoEnvironment = {
  getWorker(workerId, label) {
    return new editorWorker();
  }
};
const row = defineModel<TableColumn>('row', {
  required: true
});

// 初始化monacoEditor对象
let monacoEditor: any = null;

// 获取编辑器的内容
const handleValue = () => {
  return monacoEditor.getValue();
};

// 初始化 Monaco Editor
const initMonacoEditor = () => {
  if (!monacoEditor) {
    monacoEditor = monaco.editor.create(monacoEditorRef.value, {
      theme: 'vs-dark', // 主题 vs vs-dark hc-black
      value: '', // 默认显示的值
      language: 'csharp',
      formatOnPaste: true,
      wordWrap: 'on', // 自动换行，注意大小写
      wrappingIndent: 'indent',
      folding: true, // 是否折叠
      foldingHighlight: true, // 折叠等高线
      foldingStrategy: 'indentation', // 折叠方式  auto | indentation
      showFoldingControls: 'always', // 是否一直显示折叠 always | mouSEOver
      disableLayerHinting: true, // 等宽优化
      emptySelectionClipboard: false, // 空选择剪切板
      selectionClipboard: false, // 选择剪切板
      automaticLayout: true, // 自动布局
      codeLens: false, // 代码镜头
      scrollBeyondLastLine: false, // 滚动完最后一行后再滚动一屏幕
      colorDecorators: true, // 颜色装饰器
      accessibilitySupport: 'auto', // 辅助功能支持  "auto" | "off" | "on"
      lineNumbers: 'on', // 行号 取值： "on" | "off" | "relative" | "interval" | function
      lineNumbersMinChars: 5, // 行号最小字符   number
      readOnly: false // 是否只读  取值 true | false
    });
    // 监听编辑器内容变化
    monacoEditor.onDidChangeModelContent(() => {
      const value = handleValue();
      emits('Change', value);
      console.log('编辑器', value);
    });
  }
};

// const openDrawer = row => {
//   visible.value = true;
//   // 防止取不到
// };
// nextTick(() => {
if (monacoEditor === null) initMonacoEditor();
monacoEditor.setValue(row.value.props);
// });
const handleSubmit = () => {
  visible.value = false;
};

const closeDrawer = () => {};

// defineExpose({
//   openDrawer
// });
</script>

<template>
  <NButton type="primary" ghost size="small" @click="visible = true">重写插槽</NButton>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-800px">
    <NScrollbar class="h-480px pr-20px">
      <div id="monacoEditor" ref="monacoEditorRef" class="h-480px pr-20px"></div>
    </NScrollbar>
    <template #footer>
      <NSpace justify="end" :size="16">
        <NButton @click="closeDrawer">{{ $t('common.cancel') }}</NButton>
        <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
      </NSpace>
    </template>
  </NModal>
</template>

<style lang="less" scoped></style>
