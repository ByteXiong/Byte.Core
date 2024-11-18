<script lang="ts" setup>
import { computed } from 'vue';
import * as Enums from '@/api/apiEnums';
import { getEnumValue } from '@/utils/common';
defineOptions({
  name: 'EnumSelect',
  inheritAttrs: false
});
interface Props {
  groupBy: any;
}

const props = defineProps<Props>();

const data = computed(() => {
  return props.groupBy
    ? getEnumValue(Enums[props.groupBy as keyof typeof Enums]).map(item => ({
        label: Enums[props.groupBy as keyof typeof Enums][item],
        value: item
      }))
    : [];
});

const value = defineModel<string>('value', {
  required: true
});
</script>

<template>
  <NSelect v-model:value="value" v-bind="$attrs" placeholder="请选择枚举" :options="data" clearable />
</template>
