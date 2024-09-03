<template>
  <div>
    <el-card title="Josn Redis拦截器">
      <el-pagination
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        v-model:current-page="page"
        v-model:page-size="pageSize"
      />
    </el-card>
  </div>
</template>

<script setup lang="ts">
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
    Apis.Demo.get_api_demo_getpage({
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
</script>
