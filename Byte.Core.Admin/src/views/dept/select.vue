<template>
  <el-tree-select
    clearable
    placeholder="请选择组织"
    v-bind="$attrs"
    @change="change"
    :data="data"
    check-strictly
    :render-after-expand="false"
    :props="{ label: 'name', value: 'id' }"
  />
</template>
<script lang="ts" setup>
import { onMounted, ref } from "vue";
import "@/api";

/**
 * 获取数据
 */
const { data, loading } = useRequest(
  () =>
    Apis.Dept.get_api_dept_gettreeselect({
      params: {},
      transform: (res) => {
        return res.data;
      },
    }),
  {
    immediate: true,
  }
);

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
