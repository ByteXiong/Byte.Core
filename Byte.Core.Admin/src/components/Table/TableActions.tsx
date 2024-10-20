import { defineComponent, unref, computed, PropType, ref } from "vue";
import {
  ElDropdown,
  ElDropdownMenu,
  ElDropdownItem,
  ComponentSize,
  ElIcon,
} from "element-plus";
import TableSetting from "./TableSetting.vue";
import { Refresh, Setting } from "@element-plus/icons-vue";
import { column } from "element-plus/es/components/table-v2/src/common";
import { TableColumn } from "@/api/globals";

export default defineComponent({
  name: "TableActions",
  defineOptions: {
    inheritAttrs: true,
  },
  components: {
    TableSetting,
  },
  props: {
    tableof: {
      type: String,
      required: true,
    },
  },
  emits: ["refresh", "changSize", "confirm", "tableData"],
  setup(props, { emit }) {
    // const appStore = useAppStore();
    // const { t } = useI18n();
    // const sizeMap = computed(() => appStore.sizeMap);
    const showSetting = ref(false);

    const refresh = () => {
      emit("refresh");
    };

    const changSize = (size: ComponentSize) => {
      emit("changSize", size);
    };

    // const confirm = (columns: TableColumn[]) => {
    //   emit("confirm", columns);
    // };

    const showColumnSetting = () => {
      showSetting.value = true;
    };

    return () => (
      <>
        <div class="text-right h-28px flex items-center justify-end">
          <div
            title="刷新"
            class="w-30px h-20px flex items-center justify-end"
            onClick={refresh}
          >
            <Refresh style="width: 2em; height: 2em; margin-right: 8px" />
          </div>
          <div
            title="列设置"
            class="w-30px h-20px flex items-center justify-end"
            onClick={showColumnSetting}
          >
            <Setting style="width: 2em; height: 2em; margin-right: 8px" />
          </div>
        </div>
        <TableSetting
          v-model={showSetting.value}
          tableof={props.tableof}
          onTableData={(data: TableColumn[]) => emit("tableData", data)}
        />
      </>
    );
  },
});
