<script lang="ts" setup>
import { computed } from 'vue';
import * as Enums from '@/api/apiEnums';
defineOptions({
  name: 'AllEnumSelect',
  inheritAttrs: false
});
const value = defineModel<string>('value', {});

const data = computed(() => {
  const enumNames = Object.keys(Enums);
  return enumNames.map(item => ({ label: item, value: item }));
});
</script>

<template>
  <NSelect v-model:value="value" v-bind="$attrs" :options="data" placeholder="请选择枚举" clearable />
</template>
