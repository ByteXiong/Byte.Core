import { defineComponent } from "vue";
import { ElButton, ElTable, ElTableColumn } from "element-plus";
import Actions from "./TableActions";
import { TableModelDTO } from "@/api/globals";
import { TableAlignEnum, TableFixedEnum } from "@/api/apiEnums";
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
    const tableData = ref<TableModelDTO>({});
    // 从 attrs 中解构 class 和 style，默认值为空字符串或对象
    // const { class: className = "", style = {}, ...restAttrs } = attrs;
    return () => (
      <>
        <div>
          <Actions
            tableof={props.tableof}
            onTableData={(data: TableModelDTO) => (tableData.value = data)}
          ></Actions>
          <ElTable {...attrs}>
            {tableData.value.tableColumns?.filter((column) => column.isShow).map((column, index) => (
              <ElTableColumn
                width={column.width}
                align={ TableAlignEnum[column.align||1]}
                fixed={TableFixedEnum[column.fixed||2]}
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
