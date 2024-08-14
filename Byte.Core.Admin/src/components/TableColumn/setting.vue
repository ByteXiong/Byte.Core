<template>
  <div>
    <div class="title">管理员表格列配置</div>
    <el-button @click="setcolumns"> 保存配置</el-button>
    <el-table
      v-loading="loading"
      highlight-current-row
      row-key="id"
      :data="data?.data"
      :border="true"
      ref="tableRef"
      @selection-change="handleSelectionChange"
    >
      <el-table-column
        show-overflow-tooltip
        type="selection"
        width="55"
        class-name="onExcel"
      />

      <el-table-column label="列名" prop="label" />
      <el-table-column label="字段" prop="prop" />
      <el-table-column label="模型" prop="model" />
      <el-table-column label="排序" prop="sort" />
      <el-table-column label="宽度" prop="width" />
      <template #empty>
        <el-empty description="暂无数据" />
      </template>
    </el-table>
  </div>
</template>

<script lang="tsx" setup>
import "@/api";
import { TableModel } from "@/api/globals";
const props = defineProps({
  table: {
    type: String,
    default: "TableColumnDTO",
  },
});
// 获取数据
const { data, loading } = useRequest(
  () =>
    Apis.TableColumn.get_api_tablecolumn_getcolumns({
      params: {
        Table: props.table,
        Router: "/demo",
      },
      transform: (res) => {
        emit("tableData", res.data?.data);
        return res.data;
      },
    }),
  {
    immediate: true,
  }
);
//保存配置
const { send } = useRequest(
  (data) =>
    Apis.TableColumn.post_api_tablecolumn_setcolumns({
      data,
    }),
  {
    immediate: false,
  }
);

const emit = defineEmits(["tableData"]);
const selectData = ref<TableModel[]>();
async function handleSelectionChange(e: TableModel[]) {
  selectData.value = e;
}
// 保存配置
const setcolumns = () => {
  let data = {
    Table: props.table,
    Router: "/demo",
    data: selectData.value,
  };
  emit("tableData", selectData.value);
  send(data);
};
</script>
