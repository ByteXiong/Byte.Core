<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref, watch } from 'vue';
import JsonWorker from 'monaco-editor/esm/vs/language/json/json.worker?worker';
import CssWorker from 'monaco-editor/esm/vs/language/css/css.worker?worker';
import HtmlWorker from 'monaco-editor/esm/vs/language/html/html.worker?worker';
import TsWorker from 'monaco-editor/esm/vs/language/typescript/ts.worker?worker';
import EditorWorker from 'monaco-editor/esm/vs/editor/editor.worker?worker';
import * as monaco from 'monaco-editor';
import { editorProps } from './monaco-editor-type';
export type Theme = 'vs' | 'hc-black' | 'vs-dark';
export type FoldingStrategy = 'auto' | 'indentation';
export type RenderLineHighlight = 'all' | 'line' | 'none' | 'gutter';
defineOptions({
  name: 'MonacoEditor'
});
const props = defineProps(editorProps);

interface Emits {
  (e: 'change', string: string): string;
  (e: 'editor-mounted', editor: monaco.editor.IStandaloneCodeEditor): monaco.editor.IStandaloneCodeEditor;
}
const emit = defineEmits<Emits>();
(globalThis as any).MonacoEnvironment = {
  getWorker(_: string, label: string) {
    if (label === 'json') {
      return new JsonWorker();
    }
    if (['css', 'scss', 'less'].includes(label)) {
      return new CssWorker();
    }
    if (['html', 'handlebars', 'razor'].includes(label)) {
      return new HtmlWorker();
    }
    if (['typescript', 'javascript'].includes(label)) {
      return new TsWorker();
    }
    return new EditorWorker();
  }
};
let editor: monaco.editor.IStandaloneCodeEditor;
const codeEditBox = ref();
const modelValue = defineModel<string>('value', {
  required: true
});
const init = () => {
  monaco.languages.typescript.javascriptDefaults.setDiagnosticsOptions({
    noSemanticValidation: true,
    noSyntaxValidation: false
  });
  monaco.languages.typescript.javascriptDefaults.setCompilerOptions({
    target: monaco.languages.typescript.ScriptTarget.ES2020,
    allowNonTsExtensions: true
  });
  editor = monaco.editor.create(codeEditBox.value, {
    value: modelValue.value,
    language: props.language,
    theme: props.theme,
    ...props.options
  });
  editor.onDidChangeModelContent(() => {
    modelValue.value = editor.getValue();
    emit('change', modelValue.value);
  });
  emit('editor-mounted', editor);
};
watch(
  () => modelValue.value,
  newValue => {
    if (editor) {
      const value = editor.getValue();
      if (newValue !== value) {
        editor.setValue(newValue);
      }
    }
  }
);
watch(
  () => props.options,
  newValue => {
    editor.updateOptions(newValue);
  },
  { deep: true }
);
watch(
  () => props.language,
  newValue => {
    monaco.editor.setModelLanguage(editor.getModel()!, newValue);
  }
);
onBeforeUnmount(() => {
  editor.dispose();
});
onMounted(() => {
  init();
});
</script>

<template>
  <div ref="codeEditBox" class="codeEditBox"></div>
</template>

<style lang="scss" scoped>
.codeEditBox {
  width: v-bind(width);
  height: v-bind(height);
}
</style>
