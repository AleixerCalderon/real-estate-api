'use client';

import { useState, useEffect } from 'react';
import { propertyApi } from '@/services/api';
import { Wifi, WifiOff, RefreshCw } from 'lucide-react';

export function ConnectionStatus() {
  const [isConnected, setIsConnected] = useState<boolean | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const testConnection = async () => {
    setIsLoading(true);
    try {     
      const response = await propertyApi.getAll();
      setIsConnected(response.success);
    } catch (error) {
      console.error('Connection test failed:', error);
      setIsConnected(false);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    testConnection();
  }, []);

  if (isLoading) {
    return (
      <div className="flex items-center space-x-2 text-sm text-gray-600">
        <RefreshCw className="w-4 h-4 animate-spin" />
        <span>Verificando conexión...</span>
      </div>
    );
  }

  if (isConnected === false) {
    return (
      <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-4">
        <div className="flex items-center">
          <WifiOff className="h-5 w-5 text-red-400 mr-2" />
          <div className="flex-1">
            <h3 className="text-red-800 font-medium">Error de Conexión</h3>
            <p className="text-red-700 text-sm mt-1">
              No se puede conectar con la API. Verifica que el backend esté ejecutándose.
            </p>
            <div className="mt-2 text-xs text-red-600">
              URL: {process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5107/api'}
            </div>
            <button
              onClick={testConnection}
              className="mt-2 bg-red-600 text-white px-3 py-1 rounded text-sm hover:bg-red-700 transition-colors"
            >
              Reintentar
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="flex items-center space-x-2 text-sm text-green-600">
      <Wifi className="w-4 h-4" />
      <span>API Conectada</span>
    </div>
  );
}