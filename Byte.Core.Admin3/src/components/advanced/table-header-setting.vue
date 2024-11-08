<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useRequest } from 'alova/client';
import { $t } from '@/locales';
import type { TableColumn } from '@/api/globals';

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

const searchData = defineModel<TableColumn[]>('searchData', {
  default: () => []
});

// const jsonString =
//   '{"name": "example", "renderFunction": "function render(row, submit) { return `<div>${row.isShow ? \'显示\' : \'隐藏\'}</div>`; }"}';
// const parsedObject = JSON.parse(jsonString);
// const renderFunction = new Function(`return ${parsedObject.renderFunction}`)();

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
            // render: renderFunction as VNodeChild
            // render: (row: any, index: number) => {
            //   return eval(retrievedRenderFunctionString || '');
            // }
          } as NaiveUI.TableColumnCheck;
        });
        searchData.value = res.data?.filter(item => item.searchType);
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
