<script setup lang="tsx">
import { computed, ref } from 'vue';
import { usePagination, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import type { DataTableColumn, DataTableSortState } from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import customRender from '@/utils/customRender';
import { useAuth } from '@/hooks/business/auth';
import { ViewTypeEnum } from '@/api/apiEnums';
import EditForm from './modules/editForm.vue';
const { hasAuth } = useAuth();
// 获取当前页面路由参数
const route = useRoute();
const tableof = ref('UserDTO');
const searchParams = ref<NaiveUI.SearchParams>({});
const keyWord = ref('');
const sortList = ref<Record<string, string>>({ id: 'asc' });
/** 获取数据 */
const {
  data,
  page,
  pageSize,
  total: itemCount,
  loading,
  send: getData,
  reload
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (upPageIndex, upPageSize) =>
    Apis.User.get_api_user_getpage({
      params: {
        PageIndex: upPageIndex,
        pageSize: upPageSize,
        sortList: sortList.value,
        search: searchParams.value
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
    force: true,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: res => res.data?.pagerInfo?.totalRowCount,
    data: res => res.data?.data
  }
);
// 删除
const { send: handleDelete } = useRequest(
  ids =>
    Apis.User.delete_api_user_delete({
      data: ids,
      transform: res => {
        window.$message?.success('删除成功！');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

const appStore = useAppStore();
const handleSorter = (options: DataTableSortState) => {
  sortList.value = {};
  if (options.order) {
    sortList.value[options.columnKey] = options.order === 'descend' ? 'asc' : 'desc';
  } else {
    sortList.value.id = 'asc';
  }
};

const checkedRowKeys = ref<string[]>([]);

// 打开编辑/新增
const editFormRef = ref();
const openForm = (id?: string) => {
  editFormRef.value?.openForm(id);
};

// ====================开始处理动态生成=====================
// 共享函数
defineExpose({
  openForm,
  handleDelete
});
const searchData = ref<Array<any>>([]);
const columns = ref<Array<NaiveUI.TableColumnCheck>>([]);
const columnData = computed<Array<DataTableColumn>>(() => {
  return columns.value
    ?.filter(item => item.checked)
    .map(item => {
      const column = customRender(item.props);
      return {
        ...column,
        key: item.key,
        title: item.title,
        sorter: true
      } as DataTableColumn;
    });
});
</script>

<template>
  <div class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <TableHeaderSearch
      v-model:search-params="searchParams"
      :search-data="searchData"
      @reset="reload"
      @search="getData(1, pageSize)"
    />

    <NCard
      :title="$t(route.meta.i18nKey || route.meta.title || '')"
      :bordered="false"
      size="small"
      class="sm:flex-1-hidden card-wrapper"
    >
      <template #header-extra>
        <TableHeaderOperation
          v-model:columns="columns"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="openForm"
          @delete="handleDelete(checkedRowKeys)"
          @refresh="reload"
        >
          <template #prefix>
            <TableHeaderSetting
              v-model:columns="columns"
              v-model:search-data="searchData"
              :tableof="tableof"
              :view-type="ViewTypeEnum.主页"
            ></TableHeaderSetting>
          </template>
          <NButton v-if="hasAuth('user/submit')" size="small" ghost type="primary" @click="openForm()">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <NPopconfirm v-if="hasAuth('user/delete')" @positive-click="handleDelete">
            <template #trigger>
              <NButton size="small" ghost type="error" :disabled="checkedRowKeys?.length === 0">
                <template #icon>
                  <icon-ic-round-delete class="text-icon" />
                </template>
                {{ $t('common.batchDelete') }}
              </NButton>
            </template>
            {{ $t('common.confirmDelete') }}
          </NPopconfirm>
        </TableHeaderOperation>
      </template>
      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columnData"
        :data="data"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="1088"
        :loading="loading"
        :row-key="row => row.id"
        remote
        class="sm:h-full"
        :on-update:sorter="handleSorter"
        :pagination="{
          page,
          pageSize,
          showSizePicker: true,
          itemCount,
          pageSizes: [10, 50, 100, 500, 1000],
          onUpdatePage(value) {
            page = value;
          },
          onUpdatePageSize(value) {
            page = 1;
            pageSize = value;
          }
        }"
      />
    </NCard>
    <EditForm ref="editFormRef"></EditForm>
  </div>
</template>

<style scoped></style>
