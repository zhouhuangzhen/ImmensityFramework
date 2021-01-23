//  系统信息管理

import Layout from '@/layout'

const componentsRouter = {
  path: '/systeminfo',
  component: Layout,
  redirect: 'noRedirect',
  name: 'SystemInfo',
  meta: {
    title: 'systeminfo',
    icon: 'component'
  },
  children: [
    {
      path: 'catalog',
      component: () => import('@/views/system-codegenerator/system-manage/catalog'),
      name: 'Catalog',
      meta: { title: 'catalog' }
    },
    {
      path: 'catalogdetail',
      component: () => import('@/views/system-codegenerator/system-manage/catalogdetail'),
      name: 'CatalogDetail',
      meta: { title: 'catalogdetail' }
    }
  ]
}

export default componentsRouter
