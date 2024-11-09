<script setup lang="tsx">
import type * as monaco from 'monaco-editor';
import type { VNodeChild } from 'vue';
import { compile, h, ref, render } from 'vue';
import  from 'naive-ui';
import { $t } from '@/locales';
const language = ref('javascript');
const code = defineModel<string>('code', {
  required: true
});
const columns = ref<Array<NaiveUI.TableColumnCheck>>([
  {
    type: 'selection',
    align: 'center',
    checked: true
  },
  {
    key: 'name',
    title: 'name',
    align: 'center',
    checked: true,
    render: row => {}
  }
]);

const customRenderHeader = (str: VNodeChild, column: NaiveUI.TableColumnCheck, h: any) => {
  // return eval(str || '');
  return new Function(column, h)(`return ${str}`)();
};
const editorMounted = (editor: monaco.editor.IStandaloneCodeEditor) => {
  editor.onDidChangeModelContent(() => {
    const value = editor.getValue();
         
    columns.value[1].render = row => customRenderHeader(value, row, h);
    console.log(value, columns.value);
    // columns.value[1].render = row =>
    //   h(
    //     'button',
    //     {
    //       onClick: () => {
    //         const value = editor.getValue();
    //         console.log(value);
    //       }
    //     },
    //     '123'
    //   );
    // const templateCode1 = `<h1>{{ msg }}</h1>`;
  });
};
const visible = ref(false);
const title = ref('插槽编辑');

const handleSubmit = () => {
  visible.value = false;
};

const closeDrawer = () => {};
const handClick = () => {
  console.log(columns.value);
  visible.value = true;
};

const tableData = ref([
  {
    id: 1,
    name: '张三'
  },
  {
    id: 2,
    name: '李四'
  }
]);
</script>

<template>
  <NButton type="primary" ghost size="small" @click="handClick">重写插槽</NButton>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-1000px">
    <NGrid :x-gap="12" :y-gap="12" :cols="4">
      <NGi>
        {{ columns }}
        <NDataTable
          :columns="columns"
          :data="tableData"
          size="small"
          :flex-height="true"
          remote
          :row-key="row => row.id"
          class="sm:h-full"
        />
      </NGi>
      <NGi :span="3">
        {{ code }}
        <NScrollbar class="h-480px pr-20px">
          <MonacoEditor
            v-model:value="code"
            :language="language"
            width="800px"
            height="480px"
            @editor-mounted="editorMounted"
          ></MonacoEditor>
        </NScrollbar>
      </NGi>
    </NGrid>

    <template #footer>
      <NSpace justify="end" :size="16">
        <NButton @click="closeDrawer">{{ $t('common.cancel') }}</NButton>
        <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
      </NSpace>
    </template>
  </NModal>
</template>

<style lang="less" scoped></style>
