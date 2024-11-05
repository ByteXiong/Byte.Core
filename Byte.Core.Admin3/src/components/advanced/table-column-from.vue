<script setup lang="ts" generic="T extends Record<string, unknown>, K = never">
import type { TableColumn } from '@/api/globals';
defineOptions({
  name: 'TableColumnFrom'
});

interface Props {
  column: TableColumn;
}

const props = defineProps<Props>();

const visible = defineModel<boolean>('show', {
  default: false
});
</script>

<template>
  <NDrawer v-model:show="visible" :width="502" placement="left">
    <NDrawerContent :title="props?.column?.title">
      <NForm
        :model="props?.column"
        label-placement="left"
        label-width="auto"
        require-mark-placement="right-hanging"
        size="small"
      >
        <NFormItem path="key" label="字段">
          <NInput v-model:value="props.column.key" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="字段名">
          <NInput v-model:value="props.column.title" @keydown.enter.prevent />
        </NFormItem>

        <!--
 <NFormItem path="align" label="列内的文本排列">
          <NRadioGroup v-model:value="fromData.align" name="align">
            <NSpace>
              <NRadio value="left">left</NRadio>
              <NRadio value="right">right</NRadio>
              <NRadio value="center">center</NRadio>
            </NSpace>
          </NRadioGroup>
        </NFormItem>
        <NFormItem path="allowExport" label="这一列是否可以导出">
          <NSwitch v-model:value="fromData.allowExport" />
        </NFormItem>
        <NFormItem path="titleAlign" label="表头列对齐方式，若不设置该项，则使用列内的文本排列">
          <NRadioGroup v-model:value="fromData.titleAlign" name="titleAlign">
            <NSpace>
              <NRadio value="left">left</NRadio>
              <NRadio value="right">right</NRadio>
              <NRadio value="center">center</NRadio>
            </NSpace>
          </NRadioGroup>
        </NFormItem>
-->
        <!--
 <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.cellProps" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.children" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.className" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
        <NFormItem path="title" label="列内的文本排列">
          <NInput v-model:value="fromData.align" @keydown.enter.prevent />
        </NFormItem>
-->
      </NForm>
    </NDrawerContent>
  </NDrawer>
</template>

<style scoped></style>
