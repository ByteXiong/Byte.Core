<script lang="tsx" setup>
import type { DataTableRowKey } from 'naive-ui';
import JobList from './modules/job_list.vue';
import TriggerList from './modules/trigger_list.vue';

const handleCheck = (rowKeys: DataTableRowKey[]) => {
  console.log(rowKeys);
};
</script>

<template>
  <NGrid x-gap="12" :cols="4">
    <NGi span="1">
      <JobList @handle-check="handleCheck" />
    </NGi>
    <NGi span="3">
      <TriggerList />
    </NGi>
  </NGrid>
</template>
