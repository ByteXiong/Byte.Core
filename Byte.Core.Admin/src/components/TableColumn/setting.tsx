import { defineComponent } from "vue";
import { ElTable, ElTableColumn, ElButton } from "element-plus";
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
  const data = {
    Table: props.table,
    Router: "/demo",
    data: selectData.value,
  };
  emit("tableData", selectData.value);
  send(data);
};

export default defineComponent({
  name: "TableSetting",
  props: {
    data: {
      type: String,
      default: "TableColumnDTO",
    },
  },
  emits: ["tableData"],
  render() {
    return (
      <>
        {" "}
        <ElButton type="primary" onClick={setcolumns}></ElButton>
      </>
    );
  },
});
