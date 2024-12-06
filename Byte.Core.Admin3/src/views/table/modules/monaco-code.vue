<script setup lang="tsx">
import type * as monaco from 'monaco-editor';
import { ref } from 'vue';
import type { DataTableColumn } from 'naive-ui';
import { $t } from '@/locales';
import customRender from '@/utils/customRender';
const language = ref('javascript');
const code = defineModel<string>('code', {
  required: true
});
interface Emits {
  (e: 'Change', value: string): string;
}
const value = ref<string>('');
const emit = defineEmits<Emits>();
const columns = ref<Array<DataTableColumn>>([
  {
    type: 'selection',
    align: 'center'
  },
  {
    key: 'name',
    title: 'name',
    align: 'center'
    // render: row => {
    //   return h('p', Enum.MenuTypeEnum[row.id]);
    // }
  }
]);

const editorMounted = (editor: monaco.editor.IStandaloneCodeEditor) => {
  editor.onDidChangeModelContent(() => {
    // const value = editor.getValue();
    // columns.value[2] = customRender(value || '', h, naive)[0];
  });
};
const visible = ref(false);
const title = ref('插槽编辑');

const handleSubmit = () => {
  visible.value = false;
  emit('Change', value.value);
};

const closeModal = () => {
  visible.value = false;
};
const handClick = () => {
  visible.value = true;
  value.value = code.value;
};
const loading = ref(false);
const handlTest = () => {
  loading.value = true;
  try {
    columns.value[2] = customRender(value.value);
  } catch (e) {
    console.error(e);
  }

  loading.value = false;
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
  <NButton type="primary" :ghost="!code || code.length == 0" size="small" @click="handClick">重写插槽</NButton>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-1400px">
    <NGrid :x-gap="12" :y-gap="12" :cols="4">
      <NGi>
        {{ columns }}
        <NDataTable
          :columns="columns"
          :data="tableData"
          size="small"
          remote
          :row-key="row => row.id"
          :loading="loading"
        />
      </NGi>
      <NGi :span="3">
        <NScrollbar class="h-750px pr-20px">
          <MonacoEditor
            v-model:value="value"
            :language="language"
            width="1000px"
            height="750px"
            @editor-mounted="editorMounted"
          ></MonacoEditor>
        </NScrollbar>
      </NGi>
    </NGrid>

    <template #footer>
      <NSpace justify="space-between" :size="16">
        <NButton type="primary" @click="handlTest">{{ $t('测试代码') }}</NButton>
        <NSpace justify="end" :size="16">
          <NButton type="primary" @click="closeModal">{{ $t('common.cancel') }}</NButton>
          <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
        </NSpace>
      </NSpace>
    </template>
  </NModal>
</template>

<style lang="less" scoped></style>
