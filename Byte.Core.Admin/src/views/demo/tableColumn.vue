<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <div class="flex justify-between mb-2">
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" :content="'新增'" placement="top">
            <!-- <el-button type="primary"  icon="Plus"  circle @click="openForm()" >  </el-button> -->
          </el-tooltip>
          <el-tooltip effect="light" content="批量删除" placement="top">
            <el-icon @click="handleDelete(selectIds)"><Delete /></el-icon>
          </el-tooltip>

          <!-- <el-button type="danger" :disabled="selectIds.length==0"  @click="handleDelete(selectIds)">批量删除</el-button> -->
        </div>
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" content="刷新" placement="top">
            <el-icon @click="reload"><Refresh /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="导出" placement="top">
            <el-icon><Download /></el-icon>
          </el-tooltip>

          <!-- <el-tooltip effect="light" content="设置表头" placement="top">
            <el-icon><Setting /></el-icon>
          </el-tooltip> -->
        </div>
        <!-- <el-button type="danger" icon="Delete" circle  :disabled="selectIds.length==0"  @click="handleDelete(selectIds)"/> -->

        <!-- <el-col :span="1" class="text-align-right">
            <excel tid="table_excel_WareHouse" :name="head.comment" />
          </el-col>
          <el-col :span="1" class="text-align-right">
            <head-seting v-model="head" name="WareHouseDTO" />
          </el-col> -->
      </div>
      <tableColumn
        :data="data"
        tableof="TableColumnDTO"
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
import TableColumn from "@/components/TableColumn/index";
import "@/api";
defineOptions({
  name: "DemoTableColumn",
  inheritAttrs: true,
});
const sortList = ref<Record<string, string>>({ id: "asc" });
/**
 * 获取数据
 */
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
    Apis.TableColumn.get_api_tablecolumn_getpage({
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
