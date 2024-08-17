import { defineComponent } from "vue";
import {
  ElTable,
  ElTableColumn,
  ElButton,
  ElInput,
  ElSwitch,
  ElInputNumber,
  ElSelect,
  ElOption,
  ElTag,
} from "element-plus";
import { TableColumn, TableModel } from "@/api/globals";
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
    const { data: tableMdoel, loading } = useWatcher(
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
        debounce: 500,
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
      emit("tableData", data);
      send(data);
    };

    return () => (
      <>
        <ElTable
          data={tableMdoel.value?.data}
          v-loading={loading.value}
          highlightCurrentRow={true}
          rowKey="id"
          border={true}
          onSelection-change={(e: any) => (selectData.value = e)}
        >
          <ElTableColumn
            showOverflowTooltip
            type="selection"
            width="55"
            class-name="onExcel"
          />

          <ElTableColumn label="列名" prop="label" width={120}>
            {{
              default: ({ row }: any) => (
                <ElInput v-model={row.label} placeholder="请输入列名"></ElInput>
              ),
            }}
          </ElTableColumn>

          <ElTableColumn label="字段名" prop="prop" width={120}>
            {{
              default: ({ row }: any) => (
                <ElInput
                  v-model={row.prop}
                  placeholder="请输入字段名"
                ></ElInput>
              ),
            }}
          </ElTableColumn>

          <ElTableColumn label="隐藏" prop="isHidden" width={80}>
            {{
              default: ({ row }: any) => (
                <ElSwitch v-model={row.isHidden}></ElSwitch>
              ),
            }}
          </ElTableColumn>
          <ElTableColumn label="排序" prop="sortable" width={80}>
            {{
              default: ({ row }: any) => (
                <ElSwitch v-model={row.sortable}></ElSwitch>
              ),
            }}
          </ElTableColumn>

          <ElTableColumn label="宽度" prop="width" width={80}>
            {{
              default: ({ row }: any) => (
                <ElInputNumber
                  controls={false}
                  v-model={row.width}
                  placeholder="请输入宽度"
                ></ElInputNumber>
              ),
            }}
          </ElTableColumn>
          <ElTableColumn label="排序" prop="sort">
            {{
              default: ({ row }: any) => (
                <ElInputNumber
                  controls={false}
                  v-model={row.sort}
                  placeholder="请输入排序"
                ></ElInputNumber>
              ),
            }}
          </ElTableColumn>

          <ElTableColumn label="搜索条件" prop="condition">
            {{
              default: ({ row }: any) => (
                <ElSelect v-model={row.condition}>
                  <ElOption label="自定义" value={-1}></ElOption>
                  <ElOption label="无" value={0}></ElOption>
                  <ElOption label="等于" value={1}></ElOption>
                  <ElOption label="不等于" value={2}></ElOption>
                  <ElOption label="大于" value={3}></ElOption>
                </ElSelect>
              ),
            }}
          </ElTableColumn>

          <ElTableColumn label="插槽重写" prop="condition">
            {{
              default: ({ row }: any) => (
                <>
                  <ElTag>head</ElTag>
                  <ElTag>default</ElTag>
                </>
              ),
            }}
          </ElTableColumn>
        </ElTable>
        <ElButton type="primary" onClick={setcolumns} loading={loading.value}>
          保存
        </ElButton>
      </>
    );
  },
});
