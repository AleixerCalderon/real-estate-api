'use client';

import { useState, useEffect } from 'react';
import { Property, PropertyFilter } from '@/types';
import { propertyApi, formatPrice } from '@/services/api';
import { PropertyCard } from '@/components/PropertyCard';
import { SearchFilters } from '@/components/SearchFilters';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { PropertyDetail } from '@/components/PropertyDetail';
import { ErrorMessage } from '@/components/ErrorMessage';
import { EmptyState } from '@/components/EmptyState';
import { Pagination } from '@/components/Pagination';
import { StatsCard } from '@/components/StatsCard';
import { ConnectionStatus } from '@/components/ConnectionStatus';
import { Home, DollarSign, Calendar, TrendingUp, AlertTriangle, Plus } from 'lucide-react';
import { PropertyForm } from '@/components/PropertyForm';
import { OwnerForm } from '@/components/OwnerForm';

export default function HomePage() {
  const [properties, setProperties] = useState<Property[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedProperty, setSelectedProperty] = useState<Property | null>(null);
  const [showPropertyForm, setShowPropertyForm] = useState(false);
  const [showOwnerForm, setShowOwnerForm] = useState(false);
  const [totalCount, setTotalCount] = useState(0);
  const [filters, setFilters] = useState<PropertyFilter>({
    page: 1,
    pageSize: 12,
  });

  const stats = {
    total: totalCount,
    averagePrice: properties.length > 0 
      ? Math.round(properties.reduce((sum, p) => sum + p.price, 0) / properties.length)
      : 0,
    newestYear: properties.length > 0 
      ? Math.max(...properties.map(p => p.year))
      : new Date().getFullYear(),
    owners: new Set(properties.map(p => p.idOwner)).size,
  };

  const totalPages = Math.ceil(totalCount / (filters.pageSize || 12));

  const loadProperties = async (searchFilters?: PropertyFilter) => {
    try {
      setLoading(true);
      setError(null);
      
      console.log('🔄 Loading properties with filters:', searchFilters || filters);
      
      const filtersToUse = searchFilters || filters;
      const response = await propertyApi.search(filtersToUse);
      
      console.log('📦 API Response:', response);
      
      if (response.success && response.data) {
        setProperties(response.data);
        setTotalCount(response.total || 0);
        console.log('✅ Properties loaded successfully:', response.data.length);
      } else {
        setError(response.message || 'Error al cargar las propiedades');
        console.error('❌ API returned error:', response.message);
      }
    } catch (err) {
      console.error('❌ Error loading properties:', err);
      const errorMessage = err instanceof Error ? err.message : 'Error de conexión con el servidor';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    console.log('🚀 Component mounted, loading properties...');
    loadProperties();
  }, []);

  const handleSearch = () => {
    setFilters(prev => ({ ...prev, page: 1 }));
    loadProperties({ ...filters, page: 1 });
  };

  const handleClearFilters = () => {
    const clearedFilters: PropertyFilter = {
      page: 1,
      pageSize: 12,
    };
    setFilters(clearedFilters);
    loadProperties(clearedFilters);
  };

  const handlePageChange = (page: number) => {
    const newFilters = { ...filters, page };
    setFilters(newFilters);
    loadProperties(newFilters);
  };

  const handleViewDetails = async (id: string) => {
    try {
      const response = await propertyApi.getById(id);
      if (response.success && response.data) {
        setSelectedProperty(response.data);
      }
    } catch (err) {
      console.error('Error loading property details:', err);
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
    
      <div className="mb-4">
        <ConnectionStatus />
      </div>

      {/* Debug Info (solo en desarrollo) AAB (Agost 20 2025)*/}
      {process.env.NODE_ENV === 'development' && (
        <div className="mb-4 p-4 bg-gray-100 rounded-lg text-sm">
          <h4 className="font-semibold mb-2">🔧 Debug Info:</h4>
          <div className="space-y-1 text-xs">
            <div>API URL: {process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5107/api'}</div>
            <div>Properties loaded: {properties.length}</div>
            <div>Total count: {totalCount}</div>
            <div>Current page: {filters.page}</div>
            <div>Loading: {loading ? 'Yes' : 'No'}</div>
            <div>Error: {error || 'None'}</div>
          </div>
        </div>
      )}
  
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          🏠 Encuentra tu Propiedad Ideal
        </h1>
        <p className="text-xl text-gray-600 max-w-2xl mx-auto mb-6">
          Explora nuestra selección de propiedades exclusivas con la mejor tecnología de búsqueda
        </p>
        
        <div className="flex justify-center gap-4">
          <button
            onClick={() => setShowPropertyForm(true)}
            className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 font-medium"
          >
            <Plus className="h-5 w-5" />
            Nueva Propiedad
          </button>
          <button
            onClick={() => setShowOwnerForm(true)}
            className="bg-green-600 text-white px-6 py-3 rounded-lg hover:bg-green-700 transition-colors flex items-center gap-2 font-medium"
          >
            <Plus className="h-5 w-5" />
            Nuevo Propietario
          </button>
        </div>
      </div>

      {!loading && properties.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <StatsCard
            title="Total de Propiedades"
            value={stats.total}
            icon={<Home className="h-6 w-6" />}
            color="blue"
          />
          <StatsCard
            title="Precio Promedio"
            value={formatPrice(stats.averagePrice)}
            icon={<DollarSign className="h-6 w-6" />}
            color="green"
          />
          <StatsCard
            title="Más Reciente"
            value={stats.newestYear}
            icon={<Calendar className="h-6 w-6" />}
            color="purple"
          />
          <StatsCard
            title="Propietarios"
            value={stats.owners}
            subtitle="únicos registrados"
            icon={<TrendingUp className="h-6 w-6" />}
            color="orange"
          />
        </div>
      )}

      <SearchFilters
        filters={filters}
        onFiltersChange={setFilters}
        onSearch={handleSearch}
        onClear={handleClearFilters}
        loading={loading}
      />

      {error && (
        <ErrorMessage
          message={error}
          onRetry={() => loadProperties()}
        />
      )}

      {loading && (
        <LoadingSpinner message="Cargando propiedades..." />
      )}

      {!loading && !error && properties.length === 0 && (
        <EmptyState
          title="No se encontraron propiedades"
          description="No hay propiedades que coincidan con los filtros aplicados. Intenta ajustar los criterios de búsqueda."
          actionLabel="Limpiar filtros"
          onAction={handleClearFilters}
        />
      )}

      {!loading && !error && properties.length > 0 && (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 mb-8">
            {properties.map((property) => (
              <PropertyCard
                key={property.id}
                property={property}
                onViewDetails={handleViewDetails}
              />
            ))}
          </div>

          <Pagination
            currentPage={filters.page || 1}
            totalPages={totalPages}
            onPageChange={handlePageChange}
            totalItems={totalCount}
            itemsPerPage={filters.pageSize || 12}
          />
        </>
      )}

      {selectedProperty && (
        <PropertyDetail
          property={selectedProperty}
          onClose={() => setSelectedProperty(null)}
        />
      )}

      {showPropertyForm && (
        <PropertyForm
          onClose={() => setShowPropertyForm(false)}
          onSuccess={() => {
            loadProperties(); 
          }}
        />
      )}

      {showOwnerForm && (
        <OwnerForm
          onClose={() => setShowOwnerForm(false)}
          onSuccess={() => {           
            console.log('Propietario creado exitosamente');
          }}
        />
      )}
    </div>
  );
}