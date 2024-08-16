import { defineComponent } from "vue";
import { ElButton, ElTable, ElTableColumn } from "element-plus";
import Setting from "./setting";
import { TableModel } from "@/api/globals";
export default defineComponent({
  defineOptions: {
    inheritAttrs: true,
  },
  name: "TableColumn",
  props: {
    tableof: {
      type: String,
      required: true,
    },
  },
  setup(props, { emit, attrs }) {
    const tableData = ref<TableModel>({});
    // 从 attrs 中解构 class 和 style，默认值为空字符串或对象
    // const { class: className = "", style = {}, ...restAttrs } = attrs;
    return () => (
      <>
        <div>
          <Setting
            tableof={props.tableof}
            onTableData={(data: TableModel) => (tableData.value = data)}
          ></Setting>
          <ElTable {...attrs}>
            {tableData.value.data?.map((column, index) => (
              <ElTableColumn
                key={index}
                label={column.label}
                prop={column.prop}
              >
                {/* <template #default="scope"> </template> */}
              </ElTableColumn>
            ))}
          </ElTable>
        </div>
      </>
    );
  },
});
