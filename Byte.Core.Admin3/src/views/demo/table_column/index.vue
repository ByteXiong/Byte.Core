<script setup lang="tsx">
import { ref } from 'vue';
import type { DataTableColumn } from 'naive-ui/es/data-table';
import { usePagination } from 'alova/client';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
const keyWord = ref('');
const sortList = ref<Record<string, string>>({ id: 'asc' });
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (page, pageSize) =>
    Apis.TableColumn.get_api_tablecolumn_getpage({
      params: {
        KeyWord: keyWord.value,
        pageIndex: page,
        pageSize,
        sortList: sortList.value
      }
    }),
  {
    watchingStates: [keyWord, sortList],
    // 请求前的初始数据（接口返回的数据格式）
    // initialData: {
    //   pagerInfo: {
    //     pageIndex: 1,
    //     pageSize: 10,
    //     totalRowCount: 0,
    //   },
    //   data: [],
    // },
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: ({ data }) => data?.pagerInfo?.totalRowCount,
    data: ({ data }) => data?.data
  }
);

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

const wrapperRef = ref<HTMLElement | null>(null);

const columns = ref<Array<DataTableColumn>>([]);

const checkedRowKeys = ref<string[]>([]);
// const { checkedRowKeys, onBatchDeleted, onDeleted } = useTableOperate(data, getData);

// const operateType = ref<OperateType>('add');

function handleAdd() {
  // operateType.value = 'add';
  // openModal();
}

async function handleBatchDelete() {
  // request
  // console.log(checkedRowKeys.value);
  // onBatchDeleted();
}

// function handleDelete(id: number) {
//   // request
//   // console.log(id);
//   // onDeleted();
// }

// /** the edit menu data or the parent menu data when adding a child menu */
// const editingData: Ref<Api.SystemManage.Menu | null> = ref(null);

// function handleEdit(item: Api.SystemManage.Menu) {
//   operateType.value = 'edit';
//   editingData.value = { ...item };

//   openModal();
// }

// function handleAddChildMenu(item: Api.SystemManage.Menu) {
//   operateType.value = 'addChild';

//   editingData.value = { ...item };

//   openModal();
// }

// const allPages = ref<string[]>([]);
// const columnChecks = ref<NaiveUI.TableColumnCheck[]>([
//   {
//     key: 'name',
//     title: $t('page.manage.menu.menuName'),
//     checked: true
//   }
// ]);
// async function getAllPages() {
//   const { data: pages } = await fetchGetAllPages();
//   allPages.value = pages || [];
// }

function init() {
  getData();
  // getAllPages();
}

// init
init();
</script>

<template>
  <div ref="wrapperRef" class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <NCard :title="$t('page.manage.menu.title')" :bordered="false" size="small" class="sm:flex-1-hidden card-wrapper">
      <template #header-extra>
        <TableHeaderOperation
          v-model:columns="columns"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="handleAdd"
          @delete="handleBatchDelete"
          @refresh="getData"
        />
      </template>
      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columns"
        :data="data"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="1088"
        :loading="loading"
        :row-key="row => row.id"
        remote
        class="sm:h-full"
        :pagination="{
          page: page,
          pageSize,
          itemCount: total,
          showSizePicker: true,
          pageSizes: [10, 20, 50, 100],
          onChange: (p: number) => {

          },
          onUpdatePageSize: (pageSize: number) => {

          }
        }"
      />
    </NCard>
  </div>
</template>

<style scoped></style>
