<template>
  <div>
    <setting
      @table-data="
        (e) => {
          data = e;
        }
      "
    />
    <el-table highlight-current-row row-key="id" :border="true" ref="tableRef">
      <el-table-column
        show-overflow-tooltip
        type="selection"
        width="55"
        class-name="onExcel"
      />
      <TableWithDynamicColumns :data="data" />
      <el-table-column
        :label="column.label"
        :prop="column.prop"
        v-for="(column, index) in data"
        :key="index"
      >
        <!-- <template #default="scope"> </template> -->
      </el-table-column>

      <template #empty>
        <el-empty description="暂无数据" />
      </template>
    </el-table>
  </div>
</template>

<script lang="tsx" setup>
import { TableColumn } from "@/api/globals";
import setting from "./setting.vue";
import { ElTable, ElTableColumn } from "element-plus";
import { render } from "nprogress";

const data = ref<TableColumn[]>();

// const TableWithDynamicColumns = defineComponent({
//   name: "TableWithDynamicColumns",
//   props: {
//     data: {
//       type: TableColumn[],
//       required: true,
//     },
//   },
//   render() {
//     return (
//       <ElTable data={this.data}>
//         {this.data.map((column, index) => (
//           <ElTableColumn key={index} label={column.label} prop={column.prop}>
//             {/* <template #default="scope"> </template> */}
//           </ElTableColumn>
//         ))}
//       </ElTable>
//     );
//   },
// });

const columns = () => {
  return [
    {
      title: "Action",
      key: "actions",
    },
  ];
};

// export default defineComponent({
//   name: "App",
//   components: {
//     TableWithDynamicColumns,
//   },
//   data() {
//     return {
//       columns: [
//         { label: "Column 1", prop: "prop1" },
//         { label: "Column 2", prop: "prop2" },
//         // Add more columns as needed
//       ],
//     };
//   },
// });
</script>
