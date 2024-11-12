<script lang="ts" setup>
import { useRequest } from '@sa/alova/client';
import type { SelectMixedOption } from 'naive-ui/es/select/src/interface';
defineOptions({
  name: 'DicSelect',
  inheritAttrs: false
});
const { loading, data } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.Role.get_api_role_getselect({
      transform: res => {
        return res.data?.map(item => {
          return {
            label: item.name,
            value: item.id
          } as SelectMixedOption;
        });
      }
    }),
  {
    force: true,
    immediate: true
  }
);
const value = defineModel<number[] | undefined>('value', {
  required: true
});
</script>

<template>
  <NSelect v-model:value="value" v-bind="$attrs" :options="data" :loading="loading" />
</template>
