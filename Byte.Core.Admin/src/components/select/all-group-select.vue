<script lang="ts" setup>
import { useRequest } from '@sa/alova/client';
defineOptions({
  name: 'AllGroupSelect',
  inheritAttrs: false
});
const value = defineModel<string>('value', {});
const { loading, data } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DicData.get_api_dicdata_getallgroupby({
      transform: res => {
        return res.data?.map(item => {
          return {
            label: item,
            value: item
          };
        });
      }
    }),
  {
    force: true,
    immediate: true
  }
);
</script>

<template>
  <NSelect
    v-model:value="value"
    v-bind="$attrs"
    :options="data"
    placeholder="请输入字典"
    :loading="loading"
    clearable
  />
</template>
