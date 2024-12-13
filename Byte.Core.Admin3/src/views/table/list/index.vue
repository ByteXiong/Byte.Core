<script setup lang="tsx">
import { computed, ref } from 'vue';
import { usePagination, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import type { DataTableColumn, DataTableSortState } from 'naive-ui';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import { alovaInstance } from '@/api';
import customRender from '@/utils/customRender';
import { ViewTypeEnum } from '@/api/apiEnums';
import { useAuth } from '@/hooks/business/auth';
import EditForm from './modules/editForm.vue';

// interface DbConfig {
//   configId: string;
// }
interface Props {
  tableof: string;
  pageUrl: string;
  delUrl?: string;
  submitConfig?: {
    tableof: string;
    info: string;
    url: string;
  };
}
const props = defineProps<Props>();
const { hasAuth } = useAuth();
const route = useRoute();
const dbConfig = computed<Props & { configId: string }>(() => {
  if (props.tableof) {
    return props as Props & { configId: string };
  }
  const config = route.query.configId as string;
  const table = route.path.split('/').pop();
  return {
    configId: config,
    tableof: table,
    pageUrl: `/api/TableColumn/GetPage/${config}/${table}`,
    delUrl: `/api/TableColumn/Delete/${config}/${table}`,
    submitConfig: {
      tableof: table,
      info: `/api/TableColumn/getInfo/${config}/${table}`,
      url: `/api/TableColumn/Submit/${config}/${table}`
    }
  } as unknown as Props & { configId: string };
});

const searchParams = ref<NaiveUI.SearchParams>({});
const keyWord = ref('');
const dataTableConfig = ref<NaiveUI.dataTableConfig>({
  sortList: {}
});
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
    alovaInstance.Get(dbConfig.value.pageUrl, {
      // Apis.TableColumn.get_api_tablecolumn_page_configid_tableof({
      // pathParams: {
      //   configId: configId.value || '',
      //   tableof: tableof.value || ''
      // },
      params: {
        PageIndex: upPageIndex,
        pageSize: upPageSize,
        sortList: dataTableConfig.value.sortList,
        search: searchParams.value
      }
    }),
  {
    watchingStates: [keyWord, dataTableConfig],
    force: false,
    immediate: false,
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 20, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: (res: any) => res.data?.pagerInfo?.totalRowCount,
    data: (res: any) => res.data?.data as any
  }
);

// 删除
const { send: handleDelete } = useRequest(
  ids =>
    // Apis.TableColumn.delete_api_tablecolumn_delete_configid_tableof({
    alovaInstance.Delete(dbConfig.value.delUrl || '', ids, {
      transform: (res: any) => {
        window.$message?.success('删除成功！');
        getData(page.value, pageSize.value);
        return res.data;
      }
    }),
  { force: true, immediate: false }
);

const handleSorter = (options: DataTableSortState) => {
  dataTableConfig.value.sortList = {};
  if (options.order) {
    dataTableConfig.value.sortList[options.columnKey] = options.order === 'descend' ? 'asc' : 'desc';
  } else {
    dataTableConfig.value.sortList.id = 'desc';
  }
};

const appStore = useAppStore();

// const { bool: visible, setTrue: openModal } = useBoolean();

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
  handleDelete,
  tableof: dbConfig.value.tableof,
  configId: dbConfig.value.configId
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
          @refresh="reload"
        >
          <template #prefix>
            <TableHeaderSetting
              v-model:columns="columns"
              v-model:search-data="searchData"
              v-model:data-table-config="dataTableConfig"
              :config-id="dbConfig.configId"
              :tableof="dbConfig.tableof"
              :view-type="ViewTypeEnum.主页"
            ></TableHeaderSetting>
          </template>
          <NButton
            v-if="hasAuth(dbConfig.submitConfig?.url?.replace('/api/', ''))"
            size="small"
            ghost
            type="primary"
            @click="openForm()"
          >
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <!--       -->
          <NPopconfirm v-if="hasAuth(dbConfig.delUrl?.replace('/api/', ''))" @positive-click="handleDelete">
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
        :row-key="(row:any) => row.id"
        remote
        class="sm:h-full"
        :on-update:sorter="handleSorter"
        :pagination="{
          page,
          pageSize,
          showSizePicker: true,
          itemCount,
          pageSizes: [20, 50, 100, 500, 1000],
          onUpdatePage(value: number) {
            page = value;
          },
          onUpdatePageSize(value: number) {
            page = 1;
            pageSize = value;
          }
        }"
      />
    </NCard>

    <EditForm
      ref="editFormRef"
      :config-id="dbConfig.configId"
      :tableof="dbConfig.tableof"
      :submit-url="dbConfig.submitConfig?.url || ''"
      :info-url="dbConfig.submitConfig?.info || ''"
      @refresh="getData(page, pageSize)"
    ></EditForm>
  </div>
</template>

<style scoped></style>
