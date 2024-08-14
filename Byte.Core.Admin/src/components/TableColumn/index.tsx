import { defineComponent } from "vue";
import { ElTable, ElTableColumn } from "element-plus";
import Setting from "./setting";
import { TableColumn } from "@/api/globals";

const data = ref<TableColumn[]>();

export default defineComponent({
  name: "YourComponent",
  props: {
    data: {
      type: Array,
      required: true,
    },
  },
  render() {
    return (
      <>
        <Setting table="123" tableData={this.data}></Setting>
        <ElTable data={this.data}>
          {this.data.map((column, index) => (
            <ElTableColumn key={index} label={column.label} prop={column.prop}>
              {/* <template #default="scope"> </template> */}
            </ElTableColumn>
          ))}
        </ElTable>
      </>
    );
  },
});
