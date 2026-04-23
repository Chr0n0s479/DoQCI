import { Routes } from '@angular/router';
import { Layout } from './layout/layout';

export const routes: Routes = [
    {
        path: '',
        component: Layout,
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/home/home').then(m => m.Home)
            },
            {
                path: 'merge',
                loadComponent: () => import('./pages/merge/merge').then(m => m.Merge)
            },
            {
                path: 'split',
                loadComponent: () => import('./pages/split/split').then(m => m.Split)
            }
        ]
    }
];
