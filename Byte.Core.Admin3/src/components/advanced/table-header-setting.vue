<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useRequest } from 'alova/client';
import { $t } from '@/locales';

const router = useRouter();
defineOptions({
  name: 'TableHeaderSetting'
});

interface Props {
  // search?: boolean;
  tableof: string;
}

const { tableof } = defineProps<Props>();

const columns = defineModel<NaiveUI.TableColumnCheck[]>('columns', {
  default: () => []
});

const { loading } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DataTable.get_api_datatable_getheader({
      params: {
        Table: tableof
      },
      transform: res => {
        columns.value = res.data?.map(item => {
          // console.log(JSON.parse(item.props || '{}'));
          return {
            key: item.key,
            title: item.title,
            checked: true
          } as NaiveUI.TableColumnCheck;
        });
      }
    }),
  {
    force: true,
    immediate: true
  }
);

// 页面跳转
function linkPush() {
  router.push({
    path: `/table/column/${tableof}`
  });
}
</script>

<template>
  <NButton v-loading="{ loading }" size="small" @click="linkPush">
    <template #icon>
      <icon-ant-design-setting-outlined class="text-icon" />
    </template>
    {{ $t('common.headerSetting') }}
  </NButton>
</template>

<style scoped></style>
