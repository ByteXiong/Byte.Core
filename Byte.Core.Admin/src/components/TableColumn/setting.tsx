import { defineComponent } from "vue";
import { ElTable, ElTableColumn, ElButton } from "element-plus";
import { TableModel } from "@/api/globals";
import "@/api";
import { useWatcher } from "alova/client";

export default defineComponent({
  defineOptions: {
    inheritAttrs: true,
  },
  name: "TableSetting",
  props: {
    tableof: {
      type: String,
      required: true,
    },
  },
  emits: ["tableData"],
  setup(props, { emit }) {
    // 获取数据
    const { data, loading } = useWatcher(
      () =>
        Apis.TableColumn.get_api_tablecolumn_getcolumns({
          params: {
            Table: props.tableof,
            Router: "/demo",
          },
          transform: (res) => {
            emit("tableData", res.data);
            return res.data;
          },
        }),
      [props],
      {
        immediate: true,
        async middleware(_, next) {
          if (props.tableof) {
            await next();
          } else {
            console.error("tableColumn组件 请配置tableof 返回模型DTO");
          }
        },
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

    const selectData = ref<TableModel[]>();
    // 保存配置
    const setcolumns = () => {
      const data = {
        Table: props.tableof,
        Router: "/demo",
        data: selectData.value,
      };
      emit("tableData", selectData.value);
      send(data);
    };

    return () => (
      <>
        <ElButton type="primary" onClick={setcolumns}>
          保存
        </ElButton>

        <ElTable
          data={data.value?.data}
          loading={loading.value}
          highlightCurrentRow={true}
          rowKey="id"
          border={true}
          onSelectionChange={(e: any) => (selectData.value = e)}
        >
          <ElTableColumn
            showOverflowTooltip
            type="selection"
            width="55"
            class-name="onExcel"
          />

          <ElTableColumn label="列名" prop="label">
            {/* <template #default="scope"> </template> */}
          </ElTableColumn>

          <ElTableColumn label="字段名" prop="prop"></ElTableColumn>

          <ElTableColumn label="是否隐藏" prop="isHidden"></ElTableColumn>

          <ElTableColumn label="是否可排序" prop="sortable"></ElTableColumn>

          <ElTableColumn label="是否可过滤" prop="condition"></ElTableColumn>
        </ElTable>
      </>
    );
  },
});
