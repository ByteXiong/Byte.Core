<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      获取 接口模型 反射动态生成头部
      <tableColumn
        :data="data"
        :tableof="tableof"
        v-loading="loading"
        highlight-current-row
        row-key="id"
        :border="true"
        @selection-change="handleSelectionChange"
        @sort-change="handleSortChange"
        ref="tableRef"
      />
      <template #footer>
        <el-pagination
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          v-model:current-page="page"
          v-model:page-size="pageSize"
        />
      </template>
    </el-card>
  </div>
</template>
<script lang="tsx" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import TableColumn from "@/components/Table/Index";
import "@/api";
defineOptions({
  name: "DemoTableColumn",
  inheritAttrs: true,
});
const sortList = ref<Record<string, string>>({ id: "asc" });
/**
 * 获取数据
 */
const tableof = ref<string>("TableColumnDTO");
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload,
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (page, pageSize) =>
    Apis.Role.get_api_role_getpage({
      params: {
        PageIndex: page,
        pageSize: pageSize,
        sortList: sortList.value,
      },
    }),
  {
    watchingStates: [sortList],
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: ({ data }) => data?.pagerInfo?.totalRowCount,
    data: ({ data }) => data?.data,
  }
);

/**
 * 删除
 */
const { send: delIds } = useRequest(
  (ids: string[]) => Apis.Menu.delete_api_menu_delete({ data: ids }),
  { immediate: false }
);

async function search() {
  await getData();
}

const selectIds = ref<string[]>([]);
//多选
async function handleSelectionChange(e: any) {
  selectIds.value = e.map((x: any) => x.id);
}
//
const handleSortChange = (data: {
  column: any;
  prop: string;
  order: string;
}) => {
  sortList.value = { [data.prop]: data.order?.replace("ending", "") };
};

// 删除
async function handleDelete(ids: string[]) {
  if (ids.length == 0) {
    ElMessage.error("请勾选批量删除条目");
    return;
  }
  ElMessageBox.confirm("确定删除吗？", "删除", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await delIds(ids);
    })
    .catch((error: any) => {
      console.log(error);
    });
}
</script>
