<template>
  <view>
    <wd-cell-group title="我的所属公司">
      <wd-cell
        :title="item.name"
        value="切换"
        v-for="(item, key) in data"
        :key="key"
      />
    </wd-cell-group>
    <wd-button type="primary" size="small"> 创建组织 </wd-button>
  </view>
</template>
<script setup lang="ts">
import "@/api";
const { data } = useRequest(
  () =>
    Apis.Dept.get_api_dept_gettree({
      transform: (res) => {
        return res.data;
      },
    }),
  {
    immediate: true,
  }
);
</script>
