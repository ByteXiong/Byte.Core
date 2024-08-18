import {
  ElDrawer,
  ElCheckbox,
  ElCheckboxGroup,
  ElText,
  ElRadioButton,
  ElRadioGroup,
  CheckboxValueType,
  ElButton,
} from "element-plus";
import ColumnSetting from "./ColumnSetting";
import { TableModel } from "@/api/globals";
import { VueDraggable } from "vue-draggable-plus";
import {
  ArrowLeft,
  ArrowRight,
  Close,
  DCaret,
  Refresh,
} from "@element-plus/icons-vue";
export default defineComponent({
  name: "TableActions",
  defineOptions: {
    inheritAttrs: true,
  },
  components: {
    ColumnSetting,
  },
  props: {
    modelValue: {
      type: Boolean,
      default: false,
    },
    tableof: {
      type: String,
      required: true,
    },
  },
  emits: ["refresh", "changSize", "confirm", "onTableData", "modelValue"],
  setup(props, { emit }) {
    // const modelValue = defineModel<boolean>();
    const modelValue = useVModel(props, "modelValue", emit);

    const tableModel = ref<TableModel>({});
    // const settingColumns = ref<TableColumn[]>()
    const checkColumns = ref<string[]>([]);

    const checkAll = ref(false);
    const isIndeterminate = ref(true);
    const handleCheckAllChange = (val: CheckboxValueType) => {
      isIndeterminate.value = false;
    };

    const handleCheckedColumnsChange = (value: CheckboxValueType[]) => {
      const checkedCount = value.length;
      // checkAll.value = checkedCount === unref(defaultCheckColumns)?.length;
      // isIndeterminate.value =
      //   checkedCount > 0 && checkedCount < unref(defaultCheckColumns)?.length;
    };
    watch(
      () => tableModel,
      (val) => {
        console.error(tableModel.value);
      }
    );

    const confirm = () => {
      modelValue.value = false;
    };

    const restore = () => {};

    return () => (
      <>
        <ElDrawer v-model={modelValue.value} title="列设置" size="350px">
          设置
          <ColumnSetting
            tableof={props.tableof}
            modelValue={tableModel.value}
            onUpdate:modelValue={(model) => (tableModel.value = model)}
          ></ColumnSetting>
          <div>
            <div class="flex items-center justify-between">
              <div class="flex items-center justify-between">
                <ElCheckbox
                  v-model={checkAll}
                  indeterminate={isIndeterminate.value}
                  onChange={handleCheckAllChange}
                />
                <ElText class="ml-8px!">
                  {checkColumns.value.length} / {tableModel.value?.data?.length}
                </ElText>
              </div>
              <ElText>固定 / 排序</ElText>
            </div>

            <VueDraggable
              modelValue={tableModel.value?.data || []}
              target=".el-checkbox-group"
              handle=".handle"
              animation={150}
            >
              {tableModel.value?.data}
              <ElCheckboxGroup
                ref="draggableWrap"
                v-model={checkColumns.value}
                onChange={handleCheckedColumnsChange}
              >
                {tableModel.value?.data?.map((item) => (
                  <>
                    <div class="flex items-center justify-between mt-12px">
                      <ElCheckbox label={item.id}>{item.label}</ElCheckbox>
                      <div class="flex items-center">
                        <ElRadioGroup size="small" v-model={item.id}>
                          <ElRadioButton label="left">
                            <ArrowLeft style="width: 1.5em; height: 1.5em" />
                          </ElRadioButton>
                          <ElRadioButton label={undefined}>
                            <Close style="width: 1.5em; height: 1.5em" />
                          </ElRadioButton>
                          <ElRadioButton label="right">
                            <ArrowRight style="width: 1.5em; height: 1.5em" />
                          </ElRadioButton>
                        </ElRadioGroup>
                        <div class="ml-12px cursor-move handle  font-size-3">
                          <DCaret style="width: 1.5em; height: 1.5em" />
                        </div>
                      </div>
                    </div>
                  </>
                ))}
              </ElCheckboxGroup>
            </VueDraggable>
          </div>
          {{
            footer: ({ row }: any) => (
              <>
                <div>
                  <ElButton onClick={restore}>还原</ElButton>
                  <ElButton type="primary" onClick={confirm}>
                    确定
                  </ElButton>
                </div>
              </>
            ),
          }}
        </ElDrawer>
      </>
    );
  },
});
