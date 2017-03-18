<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="urShopper.Cloud.Services" generation="1" functional="0" release="0" Id="316a8801-f997-43b8-a754-9969fb74412e" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="urShopper.Cloud.ServicesGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="urShopper.AdminRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/LB:urShopper.AdminRole:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="urShopper.UserRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/LB:urShopper.UserRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="urShopper.AdminRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/MapurShopper.AdminRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="urShopper.AdminRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/MapurShopper.AdminRoleInstances" />
          </maps>
        </aCS>
        <aCS name="urShopper.UserRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/MapurShopper.UserRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="urShopper.UserRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/MapurShopper.UserRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:urShopper.AdminRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRole/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:urShopper.UserRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapurShopper.AdminRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapurShopper.AdminRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRoleInstances" />
          </setting>
        </map>
        <map name="MapurShopper.UserRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapurShopper.UserRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="urShopper.AdminRole" generation="1" functional="0" release="0" software="C:\Learning\Work\LocalMVP\ursShopper\urShopper.Cloud.Services\csx\Release\roles\urShopper.AdminRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;urShopper.AdminRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;urShopper.AdminRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;urShopper.UserRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="urShopper.UserRole" generation="1" functional="0" release="0" software="C:\Learning\Work\LocalMVP\ursShopper\urShopper.Cloud.Services\csx\Release\roles\urShopper.UserRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;urShopper.UserRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;urShopper.AdminRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;urShopper.UserRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="urShopper.AdminRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="urShopper.UserRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="urShopper.AdminRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="urShopper.UserRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="urShopper.AdminRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="urShopper.UserRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="1bbe0b04-661b-45c9-8921-bc08e272cd8e" ref="Microsoft.RedDog.Contract\ServiceContract\urShopper.Cloud.ServicesContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="e8577d3e-ecf3-4401-b7f9-997dba9b1bb1" ref="Microsoft.RedDog.Contract\Interface\urShopper.AdminRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.AdminRole:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="1a0736e3-e7e6-4951-accd-85853079cf05" ref="Microsoft.RedDog.Contract\Interface\urShopper.UserRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/urShopper.Cloud.Services/urShopper.Cloud.ServicesGroup/urShopper.UserRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>