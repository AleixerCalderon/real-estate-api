import './globals.css'
import type { Metadata } from 'next'
import { Inter } from 'next/font/google'
import { Header } from '@/components/Header'

const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'Real Estate - Gestión de Propiedades',
  description: 'Sistema completo de gestión de propiedades inmobiliarias',
  keywords: 'propiedades, inmobiliaria, bienes raices, gestión',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="es">
      <body className={`${inter.className} bg-gray-50 min-h-screen`}>
        <Header />
        <main className="pb-8">
          {children}
        </main>
        <footer className="bg-white border-t border-gray-200 py-8 mt-16">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="text-center text-gray-600">
                 <p>&copy; 2025 Real Estate API. Desarrollado por Aleixer Alvarado Bernal en .NET 9 y Next.js</p>
            </div>
          </div>
        </footer>
      </body>
    </html>
  )
}
