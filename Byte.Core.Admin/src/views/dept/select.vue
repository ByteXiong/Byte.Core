<template>
  <el-tree-select
    clearable
    placeholder="请选择组织"
    v-bind="$attrs"
    @change="change"
    :data="data"
    check-strictly
    :loading="loading"
    :render-after-expand="false"
    :props="{ label: 'name', value: 'id' }"
  />
</template>
<script lang="ts" setup>
import { onMounted, ref } from "vue";
import "@/api";
import { DeptTypeEnum } from "@/api/apiEnums";
/**
 * 获取数据
 */
const { data, loading } = useRequest(
  () =>
    Apis.Dept.get_api_dept_gettreeselect({
      params: { types: props.types },
      transform: (res) => {
        return res.data;
      },
    }),
  {
    immediate: true,
  }
);

const props = defineProps({
  types: {
    type: Array as PropType<DeptTypeEnum[]>,
    default: () => [DeptTypeEnum.公司, DeptTypeEnum.部门],
  },
});

const emit = defineEmits(["getVal", "change"]);
const list = ref<any>([]);

// async function getData() {
//   let { data } = await get("/Api/menu/select");
//   list.value = [{ title: "顶级菜单", id: "", children: data }];
// }

async function change(e: string) {
  let model = list.value.find((item: any) => {
    return item.dept_Id == e;
  });
  emit("getVal", model);
  emit("change", e);
}
</script>
