<template>
  <div>
    <Actions
      :tableof="tableof"
      @table-data="(data: TableModelDTO) => (tableData = data)"
    />
    <el-table v-bind="$attrs">
      <el-tableColumn
        v-for="(column, index) in tableData.tableColumns?.filter(
          (column) => column.isShow
        )"
        :key="index"
        :width="column.width"
        :align="TableAlignEnum[column.align || 1]"
        :fixed="TableFixedEnum[column.fixed || 2]"
        :label="column.label"
        :prop="column.prop"
      >
        <template #header="{ row }">
          <component :is="customRenderHeader(row, column, h)" />
        </template>
        <template #default="{ row }">
          <component :is="customRender(row, column, h)" />
        </template>
      </el-tableColumn>
    </el-table>
  </div>
</template>
<script setup lang="ts">
import { compile, h } from "vue";
import { ElButton, ElTable, ElTableColumn } from "element-plus";
import Actions from "./TableActions";
import { TableColumn, TableModelDTO } from "@/api/globals";
import { TableAlignEnum, TableFixedEnum } from "@/api/apiEnums";

const { tableof } = defineProps<{
  tableof: string;
}>();
const tableData = ref<TableModelDTO>({});
watch(
  () => tableData.value,
  () => {
    console.log(templateCode1, render1);
    console.error(tableData.value);
  }
);
const msg = ref("Hello World!");
const templateCode1 = `<h1>{{ row.id }}</h1>`;
const render1 = compile(templateCode1);

const customRender = (row: any, column: TableColumn, h: any) => {
  return eval(column.template || "");
};
const customRenderHeader = (row: any, column: TableColumn, h: any) => {
  return eval(column.headTemplate || "");
};
</script>
