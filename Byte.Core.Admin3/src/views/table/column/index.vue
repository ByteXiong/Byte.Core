<script setup lang="tsx">
import { ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import { ColumnTypeEnum, SearchTypeEnum, getEnumValue } from '@/api/apiEnums';

import { useRoute } from 'vue-router';
import SoltModal from './modules/solt-modal.vue';
import { NButton, NCheckbox, NPopconfirm, NSelect } from 'naive-ui';
const route = useRoute();
const SoltModalRef = ref();

const {
  data: tableData,
  loading,
  send: getData
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DataTable.get_api_datatable_gettableheader({
      params: {
        Table: route.path.split('/').pop()
      },
      transform: res => {
        return res.data;
      }
    }),
  {
    force: true,
    immediate: true
  }
);

const { send: submit } = useForm(
  (_, row) =>
    Apis.DataTable.put_api_datatable_settableheader({
      data: row,
      transform: ({ data }) => {
        window.$message?.success('保存成功！');
        Object.assign(row, data);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as TableColumnHeaderDTO
  }
);

const { send: delIds } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  ids =>
    Apis.DataTable.delete_api_datatable_deletetableheader({
      data: ids,
      transform: () => {
        window.$message?.success('删除成功！');
        getData();
      }
    }),
  {
    force: true,
    immediate: true
  }
);

const appStore = useAppStore();
// const { bool: visible, setTrue: openModal } = useBoolean();

const wrapperRef = ref<HTMLElement | null>(null);

const columns = ref<Array<NaiveUI.TableColumnCheck>>([
  {
    type: 'selection',
    align: 'center',
    width: 48,
    checked: true
  },
  {
    key: 'table',
    title: $t('common.add'),
    align: 'center',
    width: 80,
    checked: true
  },
  {
    key: 'key',
    title: $t('字段'),
    align: 'center',
    checked: true
  },
  {
    key: 'title',
    title: $t('注释'),
    align: 'center',
    checked: true,
    render: row => {
      return <n-input type="text" v-model:value={row.title} placeholder="请输入备注" onChange={() => submit(row)} />;
    }
  },
  {
    key: 'isShow',
    title: $t('是否显示'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NCheckbox
          checked={row.isShow}
          checked-value={true}
          v-model:checked={row.isShow}
          onUpdate:checked={() => {
            console.log('点击row :>> ', row);
            submit(row);
          }}
        >
          {row.isShow ? '显示' : '隐藏'}
        </NCheckbox>
      );
    }
  },
  {
    key: 'searchType',
    title: $t('搜索类型'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NSelect
          v-model:value={row.searchType}
          options={getEnumValue(SearchTypeEnum).map(item => ({ label: SearchTypeEnum[item], value: item }))}
          placeholder="请选择"
          onUpdate:value={() => {
            submit(row);
          }}
        />
      );
    }
  },
  {
    key: 'columnType',
    title: $t('数据类型'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NSelect
          v-model:value={row.columnType}
          options={getEnumValue(ColumnTypeEnum).map(item => ({ label: ColumnTypeEnum[item], value: item }))}
          placeholder="请选择"
          onUpdate:value={() => {
            submit(row);
          }}
        />
      );
    }
  },
  // {
  //   key: 'sort',
  //   title: $t('排序'),
  //   align: 'center',
  //   render: row => {
  //     return <n-select v-model:value={row.type} options={options} placeholder="请选择" />;
  //   }
  // },
  {
    key: 'type',
    title: $t('操作'),
    align: 'center',
    checked: true,
    render: row => (
      <div class="flex-center gap-8px">
        <SoltModal row={row}> </SoltModal>
        {row.id !== 0 ? (
          <NPopconfirm onPositiveClick={() => delIds([row.id])}>
            {{
              default: () => $t('common.confirmDelete'),
              trigger: () => (
                <NButton type="error" ghost size="small">
                  {$t('common.delete')}
                </NButton>
              )
            }}
          </NPopconfirm>
        ) : null}
      </div>
    )
  }
]);
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
</script>

<template>
  <div ref="wrapperRef" class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <NCard :title="$t('设置表头')" :bordered="false" size="small" class="sm:flex-1-hidden card-wrapper">
      <template #header-extra>
        <TableHeaderOperation
          v-model:columns="columns"
          tableof="TableHeaderDTO"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="handleAdd"
          @delete="handleBatchDelete"
          @refresh="getData"
        />
      </template>
      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columns.filter(item => item.checked)"
        :data="tableData?.columns || []"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="702"
        :loading="loading"
        remote
        :row-key="row => row.id"
        class="sm:h-full"
      />
    </NCard>
  </div>
</template>

<style scoped></style>
