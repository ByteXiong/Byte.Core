<script lang="ts" setup>
import { useRequest } from '@sa/alova/client';
defineOptions({
  name: 'DicSelect',
  inheritAttrs: false
});
interface Props {
  groupBy: string | undefined;
}

const props = defineProps<Props>();
const { loading, data } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DicData.get_api_dicdata_getselect({
      params: {
        groupBy: props.groupBy
      },
      transform: res => {
        return res.data?.map(item => {
          return {
            label: item.label,
            value: item.value
          };
        });
      }
    }),
  {
    force: true,
    immediate: true
  }
);
const value = defineModel<string>('value', {
  required: true
});
</script>

<template>
  <NSelect v-model:value="value" v-bind="$attrs" :options="data" :loading="loading" />
</template>
