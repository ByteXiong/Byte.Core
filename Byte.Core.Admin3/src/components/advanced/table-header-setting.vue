<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useRequest } from 'alova/client';
import { $t } from '@/locales';
import type { TableColumn } from '@/api/globals';
import { ViewTypeEnum } from '@/api/apiEnums';
const router = useRouter();
defineOptions({
  name: 'TableHeaderSetting'
});

interface Props {
  configId?: string | undefined;
  tableof: string | undefined;
  viewType: ViewTypeEnum;
}

const { configId, tableof, viewType } = defineProps<Props>();

const columns = defineModel<Array<NaiveUI.TableColumnCheck>>('columns', {
  default: () => []
});

const searchData = defineModel<Array<TableColumn>>('searchData', {
  default: () => []
});

// const jsonString =
//   '{"name": "example", "renderFunction": "function render(row, submit) { return `<div>${row.isShow ? \'显示\' : \'隐藏\'}</div>`; }"}';
// const parsedObject = JSON.parse(jsonString);
// const renderFunction = new Function(`return ${parsedObject.renderFunction}`)();

const { loading } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.TableView.get_api_tableview_getview({
      params: {
        Tableof: tableof,
        type: viewType,
        ConfigId: tableof
      },
      transform: res => {
        // 为什么这里不处理 返回结果, 父级页面的方法拿不到
        columns.value =
          res.data?.tableColumns?.map(item => {
            return { ...item, checked: true } as NaiveUI.TableColumnCheck;
          }) || [];
        searchData.value = res.data?.tableColumns?.filter(item => item.searchType !== null) || [];
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
    path: `/table/column`,
    query: {
      configId,
      tableof,
      viewType
    }
  });
  // switch (viewType) {
  //   case ViewTypeEnum.主页:
  //     router.push({
  //       path: `/table/view`,
  //       query: {
  //         configId,
  //         tableof,
  //         viewType
  //       }
  //     });
  //     break;

  //   case ViewTypeEnum.详情页:
  //     router.push({
  //       path: `/table/form`,
  //       query: {
  //         configId,
  //         tableof,
  //         viewType
  //       }
  //     });
  //     break;
  //   default:
  //     break;
  // }
}
</script>

<template>
  <NButton v-loading="{ loading }" size="small" @click="linkPush">
    <template #icon>
      <icon-ant-design-setting-outlined class="text-icon" />
    </template>
    {{ ViewTypeEnum[viewType] + '配置' }}
  </NButton>
</template>

<style scoped></style>
